using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using WebApplication;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly GameDBContext _context;

        public SessionsController(GameDBContext context)
        {
            _context = context;
        }

        static int? mapid = null;
        // GET: Sessions
        public async Task<IActionResult> Index(int? id)
        {
            mapid = null;
            if (id == null) return View(await _context.Sessions.Include(s => s.Map).ToListAsync());
            mapid = id;
            return View(await _context.Sessions.Include(s => s.Map).Where(s => s.MapId == id).ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Players", new { id = session.Id, server = session.Server });
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Server,Duration,MapId")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", session.MapId);
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", session.MapId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Server,Duration,MapId")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", session.MapId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            if (_context.Players.Any(p => p.SessionId == id))
            {
                return RedirectToAction("Index", "Players", new { id = id, server = session.Server });
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                try
                                {
                                    Map newmap;
                                    Session newses;
                                    var c = (from cat in _context.Sessions
                                             where cat.Server.Contains(worksheet.Name)
                                             select cat).ToList();
                                    if (c.Count > 0)
                                    {
                                        newses = c[0];
                                    }
                                    else
                                    {
                                        newses = new Session();
                                        newses.Server = worksheet.Name;
                                        newses.Duration = Convert.ToInt32(worksheet.Cell(1, 3).Value);
                                        var m = (from map in _context.Maps
                                                 where map.Name.Contains(worksheet.Cell(1, 1).Value.ToString())
                                                 select map).ToList();
                                        if (m.Count > 0)
                                        {
                                            newmap = m[0];
                                        }
                                        else
                                        {
                                            newmap = new Map();
                                            newmap.Name = worksheet.Cell(1, 1).Value.ToString();
                                            newmap.Size = Convert.ToInt32(worksheet.Cell(1, 2).Value);
                                            _context.Maps.Add(newmap);
                                        }
                                        newses.Map = newmap;
                                        _context.Add(newses);
                                    }
                                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                    {
                                        Player player = new Player();
                                        player.Login = row.Cell(1).Value.ToString();
                                        player.Password = row.Cell(2).Value.ToString();
                                        player.Nickname = row.Cell(3).Value.ToString();
                                        player.Session = newses;
                                        _context.Players.Add(player);
                                        int maxInfo = 14, oneChar = 5;
                                        for (int i = 4; i <= maxInfo; i += oneChar)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Character character;

                                                var a = (from aut in _context.Characters
                                                         where aut.Name.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    character = a[0];
                                                }
                                                else
                                                {
                                                    character = new Character();
                                                    character.Playable = Convert.ToInt32(row.Cell(i).Value);
                                                    character.Name = row.Cell(i + 1).Value.ToString();
                                                    character.Health = Convert.ToInt32(row.Cell(i + 2).Value);
                                                    character.Stamina = Convert.ToInt32(row.Cell(i + 3).Value);
                                                    character.Backpack = Convert.ToInt32(row.Cell(i + 4).Value);
                                                    _context.Characters.Add(character);
                                                }
                                                CharacterChoose ab = new CharacterChoose();
                                                ab.Player = player;
                                                ab.Character = character;
                                                _context.CharacterChooses.Add(ab);
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log(e.ToString());
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public static void Log(string message)
        {
            System.IO.File.AppendAllText("log.txt", message);
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var sessions = _context.Sessions.Include("Players").ToList();
                if (mapid != null) sessions = _context.Sessions.Where(s => s.MapId == mapid).Include("Players").ToList();
                if (sessions.Count == 0) { var worksheet = workbook.Worksheets.Add("Empty"); }
                foreach (var c in sessions)
                {
                    var worksheet = workbook.Worksheets.Add(c.Server);

                    worksheet.Cell("A1").Value = "Логін";
                    worksheet.Cell("B1").Value = "Пароль";
                    worksheet.Cell("C1").Value = "Нікнейм";
                    worksheet.Cell("D1").Value = "Персонажі";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var players = c.Players.ToList();

                    for (int i = 0; i < players.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = players[i].Login;
                        worksheet.Cell(i + 2, 2).Value = players[i].Password;
                        worksheet.Cell(i + 2, 3).Value = players[i].Nickname;

                        var ab = _context.CharacterChooses.Where(a => a.PlayerId == players[i].Id).Include(c => c.Character).ToList();

                        int j = 0;
                        foreach (var a in ab)
                        {
                            worksheet.Cell(i + 2, j + 4).Value = a.Character.Name;
                            j++;
                        }

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"gameDB_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public async Task<IActionResult> ImportDOC(IFormFile fileWord)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileWord.FileName, false))
            {
                try
                {
                    var text = wordDocument.MainDocumentPart.Document.InnerText;
                    string [] sess = text.Split("---");
                    foreach (var cur in sess)
                    {
                        string[] sesInfo = cur.Split("*");

                        string[] map = sesInfo[0].Split(" ");
                        string[] session = sesInfo[1].Split(" ");

                        Map newmap;
                        Session newses;
                        var c = (from cat in _context.Sessions
                                 where cat.Server.Contains(session[0])
                                 select cat).ToList();
                        if (c.Count > 0)
                        {
                            newses = c[0];
                        }
                        else
                        {
                            newses = new Session();
                            newses.Server =session[0];
                            newses.Duration = Convert.ToInt32(session[1]);
                            var ma = (from m in _context.Maps
                                     where m.Name.Contains(map[0])
                                     select m).ToList();
                            if (ma.Count > 0)
                            {
                                newmap = ma[0];
                            }
                            else
                            {
                                newmap = new Map();
                                newmap.Name = map[0];
                                newmap.Size = Convert.ToInt32(map[1]);
                                _context.Maps.Add(newmap);
                            }
                            newses.Map = newmap;
                            _context.Add(newses);
                        }

                        for(int i=2; i<sesInfo.Length; i++)
                        {
                            string[] playerAndCharacters = sesInfo[i].Split(": ");
                            string[] play = playerAndCharacters[0].Split(" ");
                            string[] characters = playerAndCharacters[1].Split(" | ");

                            Player player = new Player();
                            player.Login = play[0];
                            player.Password = play[1];
                            player.Nickname = play[2];
                            player.Session = newses;
                            _context.Players.Add(player);
                            foreach (var cure in characters)
                            {
                                string[] cha = cure.Split(" "); 
                                    Character character;

                                    var a = (from aut in _context.Characters
                                             where aut.Name.Contains(cha[1])
                                             select aut).ToList();
                                    if (a.Count > 0)
                                    {
                                        character = a[0];
                                    }
                                    else
                                    {
                                        character = new Character();
                                        character.Playable = Convert.ToInt32(cha[0]);
                                        character.Name = cha[1];
                                        character.Health = Convert.ToInt32(cha[2]);
                                        character.Stamina = Convert.ToInt32(cha[3]);
                                        character.Backpack = Convert.ToInt32(cha[4]);
                                        _context.Characters.Add(character);
                                    }
                                    CharacterChoose ab = new CharacterChoose();
                                    ab.Player = player;
                                    ab.Character = character;
                                    _context.CharacterChooses.Add(ab);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExportDOC ()
        {
            var stream = new MemoryStream();
            using (WordprocessingDocument doc = WordprocessingDocument.Create(stream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                new Document(new Body()).Save(mainPart);

                Body body = mainPart.Document.Body;

                var sessions = _context.Sessions.Include("Players").ToList();
                if (mapid != null) sessions = _context.Sessions.Where(s => s.MapId == mapid).Include("Players").ToList();

                foreach (var c in sessions)
                {
                    body.Append(new Paragraph(new Run(new Text(c.Server))));

                    Table table = new Table();

                    TableProperties tblProp = new TableProperties(
                        new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 }),
                        new TableStyle() { Val = "TableGrid" },
                        new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }
                    );
                    table.AppendChild<TableProperties>(tblProp);

                    TableRow tr = new TableRow();

                    

                    TableCell tc1 = new TableCell(new Paragraph(new Run(new Text("Логін"))));
                    var tcp1 = new TableCellProperties(new Shading() { Color = "auto", Fill = "a8a8a8", Val = ShadingPatternValues.Clear });
                    tc1.Append(tcp1);
                    TableCell tc2 = new TableCell(new Paragraph(new Run(new Text("Пароль"))));
                    var tcp2 = new TableCellProperties(new Shading() { Color = "auto", Fill = "b3b3b3", Val = ShadingPatternValues.Clear });
                    tc2.Append(tcp2);
                    TableCell tc3 = new TableCell(new Paragraph(new Run(new Text("Нікнейм"))));
                    var tcp3 = new TableCellProperties(new Shading() { Color = "auto", Fill = "c4c4c4", Val = ShadingPatternValues.Clear });
                    tc3.Append(tcp3);
                    TableCell tc4 = new TableCell(new Paragraph(new Run(new Text("Персонажі"))));
                    var tcp4 = new TableCellProperties(new Shading() { Color = "auto", Fill = "d1d1d1", Val = ShadingPatternValues.Clear });
                    tc4.Append(tcp4);

                    tr.Append(tc1, tc2, tc3, tc4);
                    table.Append(tr);
        
                    var players = c.Players.ToList();

                    for (int i = 0; i < players.Count; i++)
                    {
                        TableRow tr1 = new TableRow();
                        tc1 = new TableCell(new Paragraph(new Run(new Text(players[i].Login))));
                        tc2 = new TableCell(new Paragraph(new Run(new Text(players[i].Password))));
                        tc3 = new TableCell(new Paragraph(new Run(new Text(players[i].Nickname))));
                        tr1.Append(tc1, tc2, tc3);

                        var ab = _context.CharacterChooses.Where(a => a.PlayerId == players[i].Id).Include(c => c.Character).ToList();
                        string charac = "";
                        foreach (var a in ab)
                        {
                            charac += a.Character.Name;
                        }
                        tc4 = new TableCell(new Paragraph(new Run(new Text(charac))));
                        tr1.Append(tc4);

                        table.Append(tr1);
                    }

                    body.Append(table);

                    body.Append(new Paragraph(new Run(new Text("\n\n"))));
                }

                doc.MainDocumentPart.Document.Save();

                mainPart.Document.Save();

                doc.Close();
            }
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"gameDB_{DateTime.UtcNow.ToShortDateString()}.docx");
        }

    }
}


