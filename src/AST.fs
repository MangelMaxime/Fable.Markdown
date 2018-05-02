module Fable.Markdown.AST

/// A list kind can be `Ordered` or `Unordered` corresponding to `<ol>` and `<ul>` elements
type MarkdownListKind =
    | Ordered
    | Unordered

/// Column in a table can be aligned to left, right, center or using the default alignment
type MarkdownColumnAlignment =
    | AlignLeft
    | AlignRight
    | AlignCenter
    | AlignDefault

/// Represents inline formatting inside a paragraph. This can be literal (with text), various
/// formattings (string, emphasis, etc.), hyperlinks, images, inline maths etc.
type MarkdownSpan =
    | Literal of text:string
    | InlineCode of code:string
    | Strong of body:MarkdownSpans
    | Emphasis of body:MarkdownSpans
    | AnchorLink of link:string
    | DirectLink of body:MarkdownSpans * link:string * title:option<string>
    | IndirectLink of body:MarkdownSpans * original:string * key:string
    | DirectImage of body:string * link:string * title:option<string>
    | IndirectImage of body:string * link:string * key:string
    // | HardLineBreak of range:MarkdownRange option

/// A type alias for a list of `MarkdownSpan` values
and MarkdownSpans = list<MarkdownSpan>

/// A paragraph represents a (possibly) multi-line element of a Markdown document.
/// Paragraphs are headings, inline paragraphs, code blocks, lists, quotations, tables and
/// also embedded LaTeX blocks.
type MarkdownParagraph =
    | Heading of size:int * body:MarkdownSpans
    | Paragraph of body:MarkdownSpans
    | CodeBlock of code:string * language:string * ignoredLine:string
    | InlineBlock of code:string
    | ListBlock of kind:MarkdownListKind * items:list<MarkdownParagraphs>
    | QuotedBlock of paragraphs:MarkdownParagraphs
    | Span of body:MarkdownSpans
    | HorizontalRule of character:char
    | TableBlock of headers:option<MarkdownTableRow> * alignments:list<MarkdownColumnAlignment> * rows:list<MarkdownTableRow>

/// A type alias for a list of paragraphs
and MarkdownParagraphs = list<MarkdownParagraph>

/// A type alias representing table row as a list of paragraphs
and MarkdownTableRow = list<MarkdownParagraphs>
