namespace Fable.Markdown

module Markdown =

    open Fable.Parsimmon
    open Fable.Markdown.Common
    open Fable.Markdown.AST
    open Fable.Markdown.Parser
    open System

    // let parse(input: string) =
    //     let lines =
    //         [ let mutable lineNo = 1
    //           for line in input.Split('\n') do
    //             if not (isNull line) then
    //                 yield line, { StartLine = lineNo; StartColumn = 0; EndLine = lineNo; EndColumn = line.Length }
    //                 lineNo <- lineNo + 1
    //         ]

    //     let ctx : ParsingContext =
    //         { CurrentRange =
    //             Some { StartLine = 0; StartColumn = 0; EndLine = 0; EndColumn = 0 } }

    //     let paragraphs =
    //         lines
    //         |> List.skipWhile (fun (s, _) -> String.IsNullOrWhiteSpace s)
    //         |> parseParagraphs ctx
    //         |> List.ofSeq




    // let transformHtml (text) =
    //     printfn "%s" text
    //     match parse text with
    //     | Some doc ->
    //         let sb = StringBuilder()
    //         // formatMarkdown sb doc
    //         sb.ToString()
    //     | None -> failwith "Failed to parse the given markdown document"
