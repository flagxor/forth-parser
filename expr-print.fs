#! /usr/bin/env gforth
require bnf.fs

bnf: {DIGIT}   t' 0 | t' 1 | t' 2 | t' 3 | t' 4 |
               t' 5 | t' 6 | t' 7 | t' 8 | t' 9 ;bnf

bnf: <DIGIT>   @token 48 - {DIGIT} $$ ! {{ 10 * $$ @ + }} ;bnf
bnf: <NUMBER'>   <DIGIT> <NUMBER'> | <DIGIT> ;bnf
bnf: <NUMBER> {{ 0 }} <NUMBER'> {{ . }} ;bnf

defer <'EXPRESSION>
bnf: <ELEMENT>   t' ( <'EXPRESSION> t' ) | <NUMBER> ;bnf
bnf: <PRIMARY>   t' - <PRIMARY>   {{ ." negate " }}
               | <ELEMENT> ;bnf
bnf: <FACTOR>   <PRIMARY> t' ^ <FACTOR>   {{ ." POWER " }}
              | <PRIMARY> ;bnf
bnf: <T'>   t' * <FACTOR> <T'>   {{ ." * " }}  
          | t' / <FACTOR> <T'>   {{ ." / " }}  
          | ;bnf
bnf: <TERM>   <FACTOR> <T'> ;bnf
bnf: <E'>   t' + <TERM> <E'>   {{ ." + " }} 
          | t' - <TERM> <E'>   {{ ." - " }}
          | ;bnf
bnf: <EXPRESSION>   <TERM> <E'> ;bnf
' <EXPRESSION> is <'EXPRESSION>

: parse-it <parse <EXPRESSION> <EOL> parse> ;

parse-it 123+(45+8*1111^2)
cr bye
