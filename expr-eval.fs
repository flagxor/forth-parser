require bnf.fs

bnf: {DIGIT}   t' 0 | t' 1 | t' 2 | t' 3 | t' 4 |
               t' 5 | t' 6 | t' 7 | t' 8 | t' 9 ;bnf

bnf: <DIGIT>   @token 48 - {DIGIT} $$ ! {{ 10 * $$ @ + }} ;bnf
bnf: <NUMBER'>   <DIGIT> <NUMBER'> | <DIGIT> ;bnf
bnf: <NUMBER> {{ 0 }} <NUMBER'> ;bnf

defer <'EXPRESSION>
variable x
bnf: <VARIABLE>   t' x {{ x @ }} ;bnf
bnf: <ELEMENT>   t' ( <'EXPRESSION> t' ) | <NUMBER> | <VARIABLE> ;bnf
bnf: <PRIMARY>   t' - <PRIMARY>   {{ negate }}
               | <ELEMENT> ;bnf
: pow ( x n -- n ) 1 swap 0 ?do over * loop nip ;
bnf: <FACTOR>   <PRIMARY> t' ^ <FACTOR>   {{ pow }}
              | <PRIMARY> ;bnf
bnf: <T'>   t' * <FACTOR> <T'>   {{ * }}  
          | t' / <FACTOR> <T'>   {{ / }}  
          | ;bnf
bnf: <TERM>   <FACTOR> <T'> ;bnf
bnf: <E'>   t' + <TERM> <E'>   {{ + }} 
          | t' - <TERM> <E'>   {{ - }}
          | ;bnf
bnf: <EXPRESSION>   <TERM> <E'> ;bnf
' <EXPRESSION> is <'EXPRESSION>
: expr <parse <EXPRESSION> parse> ;

( EXAMPLE )
4 x ! expr x^2+x*3+1 .
cr bye
