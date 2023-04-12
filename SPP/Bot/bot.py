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
        
async def get_most_liked_video(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    channel = update.message.text[18:]
    channelStat = get_service().channels().list(forUsername=channel, part='snippet,contentDetails,statistics').execute()

    if channelStat["pageInfo"]["totalResults"] == 0:
        await update.message.reply_text('Channel not found')
        return
    
    allUploadedVideosPlaylist =  channelStat["items"][0]['contentDetails']['relatedPlaylists']['uploads']
    videos = [ ]
    next_page_token = None

    while True:
        playlistData = get_service().playlistItems().list(playlistId=allUploadedVideosPlaylist, part='snippet', maxResults=50, pageToken=next_page_token).execute()
        videos += playlistData['items']
        next_page_token = playlistData.get('nextPageToken')

        if next_page_token is None:
            break
    
    video_ids=[]    
    for i in range(len(videos)):
        video_ids.append(videos[i]["snippet"]["resourceId"]["videoId"])
        i+=1

    videoStatistics = []
    for i in range(len(video_ids)):
        videoData = get_service().videos().list(id=video_ids[i],part = "statistics").execute()
        videoStatistics.append(videoData["items"][0]["statistics"])
        i+=1

    VideoTitle=[ ]
    url=[ ]
    Published = [ ]
    Views=[ ]
    LikeCount=[ ]
    ml = 0
    mli = 0

    for i in range(len(videos)):
        VideoTitle.append((videos[i])['snippet']['title'])
        url.append("https://www.youtube.com/watch?v="+(videos[i])['snippet']['resourceId']['videoId'])
        Published.append((videos[i])['snippet']['publishedAt'])
        Views.append(int((videoStatistics[i])['viewCount']))
        LikeCount.append(int((videoStatistics[i])['likeCount']))
        if (int((videoStatistics[i])['likeCount']) > ml):
            ml = int((videoStatistics[i])['likeCount'])
            mli = i

    await update.message.reply_text('Most liked video: ' + VideoTitle[mli] +
                                    '\nWith ' + str(ml) + ' likes' +
                                    '\nAnd ' + str(Views[mli]) + ' views' +
                                    '\nWas published: ' + Published[mli] +
                                    '\n' + url[mli])

def main() -> None:
    application = Application.builder().token(BOT_TOKEN).build()

    application.add_handler(CommandHandler("start", start))
    application.add_handler(CommandHandler("channel", get_channel_info))
    application.add_handler(CommandHandler("most_liked_video", get_most_liked_video))

    application.run_polling()


if __name__ == "__main__":
    main()
