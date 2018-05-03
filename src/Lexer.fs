module Fable.Markdown.Lexer

open Fable.Markdown.Common
open System

let inline isWhiteSpace (c:Char) =
    match int c with
    | 0x09   | 0x0B   | 0x0C
    | 0x20   | 0xA0   | 0x1680
    | 0x180E | 0x202F | 0x205F
    | 0x3000 | 0xFEFF -> true
    | c -> c >= 8192 && c <= 8202;

let inline isLineTerminator c =
    match c with
    | '\n'
    | '\r' -> true
    // Workaround https://github.com/fable-compiler/Fable/issues/1283
    // | '\u2028'
    // |'\u2029'
    | c when c = char 0x2028 -> true
    | c when c = char 0x2029 -> true
    | _ -> false

// Use [<Fable.Core.StringEnum>] for debug purpose
// This make the output more readbile for human
//  Replace with enum for performance
[<Fable.Core.StringEnum>]
type Symbol =
    // Special markers
    | StartOfInput
    | EndOfInput
    | Hash
    | LineTerminator
    | RawText
    | Space
    | Asterix

type InputStream =
    { mutable Source : string
      mutable Index : int
      mutable Line : int
      mutable Column : int
      mutable Buffer : StringBuilder }

let createInputStream source =
    { Source = source
      Index = 0
      Line = 1
      Column = 0
      Buffer = StringBuilder() }

let inline newline (input : InputStream) = input.Line <- input.Line + 1
let inline current (input : InputStream) = input.Source.[input.Index]
let inline previous (input : InputStream) = input.Source.[input.Index-1]
let inline peek (input : InputStream) = input.Source.[input.Index+1]
let inline canPeek (input : InputStream) = input.Index+1 < input.Source.Length
let inline continue' (input : InputStream) = input.Index < input.Source.Length
let inline position (input : InputStream) = input.Line, input.Column
let inline rewind (input : InputStream) = input.Index <- input.Index - 1
let inline advance (input : InputStream) =
    input.Index <- input.Index + 1
    input.Column <- input.Column + 1

let inline skip n (input : InputStream) =
    input.Index <- input.Index + n
    input.Column <- input.Column + n

let inline buffer (input : InputStream) (c:Char) = input.Buffer.Append(c) |> ignore
let inline clearBuffer (input : InputStream) = input.Buffer.Clear() |> ignore
let inline bufferValue (input : InputStream) = input.Buffer.ToString()
let inline nextLine (input : InputStream) =
    if input |> current = '\r' && input |> canPeek && input |> peek = '\n'
    then input |> advance

    input.Line <- input.Line + 1
    input.Column <- 0

let inline setupBuffer (input : InputStream) =
    input |> advance
    input |> clearBuffer
    input.Line, input.Column

let inline output symbol value line col (input : InputStream) =
    // input.Previous <- symbol
    symbol, value, line, col

let endOfInput (input : InputStream) =
    input |> output Symbol.EndOfInput null -1 -1

// let private headings (source : InputStream) =
//     let line = source.Line
//     let column = source.Column

//     source |> clearBuffer
//     (source |> current) |> buffer source

//     let rec headings (source : InputStream) =
//         source |> advance

//         if source |> continue' then
//             match source |> current with
//             | c ->
//                 c |> buffer source
//                 headings source
//         else
//             source |> advance
//             source |> output Head

//     source |> headings

[<Literal>]
let private empty:string = null

let private headings (stream : InputStream) =
    let line = stream.Line
    let column = stream.Column

    stream |> clearBuffer
    (stream |> current) |> buffer stream

    // let rec headings  (symbol : Symbol)  (stream : InputStream) =
    //     stream |> advance

    //     if stream |> continue' then
    //         match stream |> current with
    //         | '#' as c ->
    //             c |> buffer stream
    //             stream |> headings symbol
    //         | c when c |> isWhiteSpace ->
    //             let symbol =
    //                 match stream |> bufferValue with
    //                 | "#" -> Symbol.Heading1
    //                 | "##" -> Symbol.Heading2
    //                 | "###" -> Symbol.Heading3
    //                 | "####" -> Symbol.Heading4
    //                 | "#####" -> Symbol.Heading5
    //                 | "######" -> Symbol.Heading6
    //                 | _ -> failwith "Unexpected title size"

    //             stream |> clearBuffer
    //             stream |> headings symbol

    //         | c when c |> isLineTerminator ->
    //             stream |> output symbol (stream |> bufferValue) line column

    //         | c ->
    //             c |> buffer stream
    //             stream |> headings symbol
    //     else
    //         stream |> output symbol (stream |> bufferValue) line column

    // stream |> headings Symbol.Heading1

let unorderedList (stream : InputStream) =
    stream |> advance

let isValidRawText (c : char) =
    match c with
    | '#' | '*' -> false
    | c when c |> isLineTerminator -> false
    | c when c |> isWhiteSpace -> false
    | _ -> true

let rawText (stream : InputStream) =
    stream |> clearBuffer

    let rec rawText (stream : InputStream) =
        if stream  |> continue' then
            match stream |> current with
            | c when c |> isValidRawText ->
                c |> buffer stream
                stream |> advance
                stream |> rawText
            | _ ->
                stream |> output Symbol.RawText (stream |> bufferValue) stream.Line stream.Column
        else
            stream |> output Symbol.RawText (stream |> bufferValue) stream.Line stream.Column

    stream |> rawText

let tokenize (text : string) =
    let stream = createInputStream(text)

    let rec lexer () =
        if stream |> continue' then
            match stream |> current with
            | '#' ->
                stream |> advance
                stream |> output Symbol.Hash empty stream.Line stream.Column

            | '*' ->
                stream |> advance
                stream |> output Symbol.Asterix empty stream.Line stream.Column

            | c when c |> isWhiteSpace ->
                stream |> advance
                stream |> output Symbol.Space empty stream.Line stream.Column

            | c when c |> isLineTerminator ->
                stream |> nextLine
                stream |> advance
                stream |> output Symbol.LineTerminator empty stream.Line stream.Column

            | c when c |> isValidRawText -> stream |> rawText

            | c ->
                failwithf "Invalid input `%c`" c

        else
            stream |> endOfInput

    let mutable acc = []
    let mutable loop = true

    while loop do
        let (symbol, content, line, column) = lexer ()
        acc <- (symbol, content, line, column) :: acc

        if symbol = Symbol.EndOfInput then
            loop <- false

    acc |> List.rev
