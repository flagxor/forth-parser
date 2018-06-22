require bnf.fs

bnf: {DIGIT}   t' 0 | t' 1 | t' 2 | t' 3 | t' 4 |
               t' 5 | t' 6 | t' 7 | t' 8 | t' 9 ;bnf

bnf: <DIGIT>   @token 48 - {DIGIT} $$ ! {{ 10 * $$ @ + }} ;bnf
bnf: <NUMBER'>   <DIGIT> <NUMBER'> | <DIGIT> ;bnf
bnf: <NUMBER> {{ 0 }} <NUMBER'> {{ postpone literal }} ;bnf

defer <'EXPRESSION>
variable x
bnf: <VARIABLE>   t' x {{ postpone x postpone @ }} ;bnf
bnf: <ELEMENT>   t' ( <'EXPRESSION> t' ) | <NUMBER> | <VARIABLE> ;bnf
bnf: <PRIMARY>   t' - <PRIMARY>   {{ postpone negate }}
               | <ELEMENT> ;bnf
: pow ( x n -- n ) 1 swap 0 ?do over * loop nip ;
bnf: <FACTOR>   <PRIMARY> t' ^ <FACTOR>   {{ postpone pow }}
              | <PRIMARY> ;bnf
bnf: <T'>   t' * <FACTOR> <T'>   {{ postpone * }}  
          | t' / <FACTOR> <T'>   {{ postpone / }}  
          | ;bnf
bnf: <TERM>   <FACTOR> <T'> ;bnf
bnf: <E'>   t' + <TERM> <E'>   {{ postpone + }} 
          | t' - <TERM> <E'>   {{ postpone - }}
          | ;bnf
bnf: <EXPRESSION>   <TERM> <E'> ;bnf
' <EXPRESSION> is <'EXPRESSION>
: [expr] postpone ahead >r >r >r
         <parse <EXPRESSION> execute-steps
         r> r> r> postpone then execute-strip ; immediate
: expr :noname postpone [expr] postpone ; execute ;

( EXAMPLE )
: foo x ! [expr] x^2+x*3+1 ;
." DEFINITION OF foo ----------------------------" cr
see foo cr
." ----------------------------------------------" cr
." x=4 --- x^2+x*3+1 = " 4 foo . cr
." x=5 --- x^2+1 = " 5 x ! expr x^2+1 . cr
cr bye
