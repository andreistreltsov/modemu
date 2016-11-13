module Server

open System
open System.Threading
open SerialPort

type RtuServer(port:ISerialPort) = 
    let mutable serverRunning = false
    let mutable thread = null
    let buffer = Array.zeroCreate(256)
    let mutable currentIndex = 0

    let serveRequest() =
        printf "serving request of %i bytes: %A\n" currentIndex buffer.[0..currentIndex-1]

    let discardBuffer() =
        currentIndex <- 0

    let onInterframeTimeout() = 
        if currentIndex <> 0 then
            printf "interframe timer occur\n"
            serveRequest()
            discardBuffer()

    let interframeTimeoutFor baudRate = 
        if baudRate > 19200 
        then 
            ceil(1.75) |> int
        else 
            let pauseBetweenFrames = 3.5
            let msInSecond = 1000.0
            let bitsInChar = 10.0
            ceil((pauseBetweenFrames * msInSecond * bitsInChar) / float baudRate) |> int

    let interframeTimeout = interframeTimeoutFor port.BaudRate

    let interframeTimer = new Timer((fun x -> onInterframeTimeout()), null, Timeout.Infinite, Timeout.Infinite)

    let resetTimer() =
        interframeTimer.Change(0, interframeTimeout) |> ignore

    let receive() = 
        buffer.[currentIndex] <- port.Read()
        currentIndex <- currentIndex + 1
        resetTimer()

    let live() =
        while true do
            if serverRunning then
                receive()
            else 
                Thread.Sleep(1)
 
    member this.Start() = 
        if not(port.IsOpen()) then raise (new InvalidOperationException("Open the port before starting the server."))
        thread <- new Thread(live)
        thread.Start()
        serverRunning <- true