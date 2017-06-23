namespace BewerberAuswahl

type Age = Age of int
type Email = Email of string

type Candidate =
    { Name : string
    ; Age : Age
    ; Email : Option<Email>
    }

type ValidationResult<'a>
    = Valid 
    | Error of 'a
    | Skip

type Validated<'a, 'b> =
    { Value : 'a
    ; Validation : ValidationResult<'b>
    }

type ValidatedCandidate<'a> = Validated<Candidate, 'a>

type TryCreateCandidate = int -> string -> Option<string> -> Option<Candidate>

type ParseCSV = string -> List<Candidate>
type FilterCriteria<'a> = Candidate -> ValidationResult<'a>
type CombineFilters<'a> = List<FilterCriteria<'a>> -> FilterCriteria<'a>
type Filter<'a> = FilterCriteria<'a> -> List<Candidate> -> List<ValidatedCandidate<'a>>
type GenerateOutput<'a> = List<ValidatedCandidate<'a>> -> unit
