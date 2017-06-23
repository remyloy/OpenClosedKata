namespace BewerberAuswahl

type Candidate =
    { Name : string
    ; Age : int
    ; Email : Option<string>
    }

type ValidationError
    = TooOld of int
    | HasUmlaut of string

type ValidationResult
    = Valid 
    | Error of ValidationError

type Validated<'a> =
    { Value : 'a
    ; Validation : ValidationResult 
    }

type ValidatedCandidate = Validated<Candidate>

type CreateCandidate = int -> string -> Option<string> -> Candidate
type ParseCSV = string -> List<Candidate>
type FilterCriteria = Candidate -> ValidationResult
type Filter = FilterCriteria -> List<Candidate> -> List<ValidatedCandidate>
type GenerateOutput = List<ValidatedCandidate> -> unit
