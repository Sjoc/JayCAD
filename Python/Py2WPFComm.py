from win32 import win32file
from win32 import win32pipe
import time
from Image_Measuring import ImageMeasure

def Connect():
    # Python 2 pipe server (ReadThenWrite)
    pipeName="ImagServ"
    global pipe_handle
    pipe_handle = win32pipe.CreateNamedPipe(
            r'\\.\pipe\\'+pipeName,
            win32pipe.PIPE_ACCESS_DUPLEX,
            win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_READMODE_MESSAGE | win32pipe.PIPE_WAIT,
            1, 65536, 65536,
            0,
            None)
    win32pipe.ConnectNamedPipe(pipe_handle, None)
    global measuring
    measuring = ImageMeasure()

def Disconnect():
    win32file.FlushFileBuffers(pipe_handle)
    win32pipe.DisconnectNamedPipe(pipe_handle)
    win32file.CloseHandle(pipe_handle)

def OpenImate(image):
    measuring.Se


while True: 
    ret, read_message = win32file.ReadFile(pipe_handle, 1000)
    print(F'{ret = } Received from c#: ' + read_message.decode('utf-8'))

    ret, length = win32file.WriteFile(pipe_handle, 'Hello from Python\n'.encode())
    print(F'{ret = }, {length = } from WriteFile')

    win32file.FlushFileBuffers(pipe_handle)
   # win32pipe.DisconnectNamedPipe(pipe_handle)
    #win32file.CloseHandle(pipe_handle)

    time.sleep(2)