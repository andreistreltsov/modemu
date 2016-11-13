module Program

open System
open Server
open FrameworkSerialPort

[<EntryPoint>]
let main argv = 
    let port = (new FrameworkSerialPortAdapter()).Open("COM3", 9600)
    let server = new RtuServer(port)
    server.Start()
    Console.ReadLine() |> ignore
    0 
