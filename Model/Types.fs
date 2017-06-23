namespace BewerberAuswahl

type Age = Age of int
type Email = Email of string

type Candidate =
    { Name : string
    ; Age : Age
    ; Email : Option<Email>
    }

type ValidationError
    = TooOld of Age
    | HasUmlaut of string
    | Blacklisted of string

type ValidationResult
    = Valid 
    | Error of ValidationError

type Validated<'a> =
    { Value : 'a
    ; Validation : ValidationResult 
    }

type ValidatedCandidate = Validated<Candidate>

type TryCreateCandidate = int -> string -> Option<string> -> Option<Candidate>

type ParseCSV = string -> List<Candidate>
type FilterCriteria = Candidate -> ValidationResult
type CombineFilters = List<FilterCriteria> -> FilterCriteria
type Filter = FilterCriteria -> List<Candidate> -> List<ValidatedCandidate>
type GenerateOutput = List<ValidatedCandidate> -> unit
