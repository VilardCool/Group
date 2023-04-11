from googleapiclient.discovery import build
from telegram import ForceReply, Update
from telegram.ext import Application, CommandHandler, ContextTypes
from config import BOT_TOKEN, API_TOKEN
 
API_KEY = API_TOKEN
 
def get_service():
    service = build('youtube', 'v3', developerKey=API_KEY)
    return service

async def start(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    user = update.effective_user
    await update.message.reply_html(
        rf"Hi {user.mention_html()}!",
        reply_markup=ForceReply(selective=True),
    )

async def get_channel_info(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    channel = update.message.text[9:]
    r = get_service().channels().list(forUsername=channel, part='snippet,statistics').execute()
    if r["pageInfo"]["totalResults"] == 0:
        await update.message.reply_text('Channel not found')
        return
    else:
        await update.message.reply_text(channel + ' channel was created: ' + r['items'][0]['snippet']['publishedAt'] + 
                                        '\nViews: ' + r['items'][0]['statistics']['viewCount'] + 
                                        '\nSubscribers: ' + r['items'][0]['statistics']['subscriberCount'] + 
                                        '\nVideos: ' + r['items'][0]['statistics']['videoCount'])

def main() -> None:
    application = Application.builder().token(BOT_TOKEN).build()

    application.add_handler(CommandHandler("start", start))
    application.add_handler(CommandHandler("channel", get_channel_info))

    application.run_polling()


if __name__ == "__main__":
    main()
