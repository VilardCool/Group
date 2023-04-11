from telegram.ext import Updater, CommandHandler, MessageHandler, filters
import requests
from config import BOT_TOKEN

def movie_handler(update, context):
    id = int(update.message.from_user.id)

    print(id)
    first_name = update.message.chat.first_name
    opening_line = f"""Hi {first_name}!"""
    update.message.reply_text(opening_line)

    chat_id = update.message.chat_id

    movie_name = update.message.text[7:].strip()

    context.bot.send_message(chat_id=chat_id, text="No movie available for this title: " + movie_name)

async def help_command(update, context):
    await update.message.reply_text("Help!")

async def echo(update, context):
    await update.message.reply_text(update.message.text)


def main():
    updater = Updater(BOT_TOKEN, use_context=True)
    dispatcher = updater.dispatcher

    dispatcher.add_handler(CommandHandler("movie", movie_handler))

    dispatcher.add_handler(CommandHandler("help", help_command))

    dispatcher.add_handler(MessageHandler(filters.TEXT & ~filters.COMMAND, echo))

    updater.start_polling()
    updater.idle()


if __name__ == "__main__":
    main()
