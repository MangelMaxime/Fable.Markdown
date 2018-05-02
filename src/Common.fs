module Fable.Markdown.Common

/// Custom implementation for a StringBuilder
/// We need to have a Class instance referenced in the context in order to mutable
/// the builder from everywhere
type StringBuilder () =
    let mutable output = ""

    member __.Write (text) = output <- output + text

    member __.Append (text : string) = output <- output + text

    member __.Append (c : char) = output <- output + string c

    member __.Clear () = output <- ""

    member __.WriteLine (?text : string) =
        match text with
        | Some text -> output <- output + text + "\n"
        | None -> output <- output + "\n"

    override __.ToString() = output

type Range =
    { StartLine : int
      StartColumn : int
      EndLine : int
      EndColumn : int }
