import base64
import sys
from Crypto.Cipher import AES
from pkcs7 import PKCS7Encoder

def readFile():
    filePath = sys.argv[1]
    fhand = open(filePath)
    message = ''
    for line in fhand:
        message = message + line
    return message

def decryptFile(myMessage):
    key = 'A16ByteKey......'
    mode = AES.MODE_CBC
    iv = '\x00' * 16
    encoder = PKCS7Encoder()
    e = AES.new(key.encode("utf8"),mode,iv.encode("utf8"))
    firstStep = base64.b64decode(myMessage)
    cipher_text = e.decrypt(firstStep).decode("utf8")
    decryptedString = encoder.decode(cipher_text)
    return decryptedString

def saveDecryptFile(decryptedContent):
    destinationPath = sys.argv[2]
    fout = open(destinationPath,'w')
    fout.write(decryptedContent)
    fout.close()

def main():
    fileContent = readFile()
    decryptedMessage = decryptFile(fileContent)
    saveDecryptFile(decryptedMessage)
    
if __name__=="__main__":
    main()