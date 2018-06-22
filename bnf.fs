\ Based on http://www.bradrodriguez.com/papers/bnfparse.htm
\ And https://github.com/letoh/fina-forth/blob/master/bnf.fs

variable success
: dp! ( a -- ) here - allot ;
: <bnf ( -- ) success @ if r> >in @ >r here >r >r else r> drop then ;
: bnf> ( -- ) success @ if r> r> r> 2drop >r else r> r> dp! r> >in ! >r then ;
: | ( -- ) success @ if r> r> r> 2drop drop
                     else r> r> r> 2dup >r >r >in ! dp!  1 success ! >r then ;
: bnf: ( -- sys )  : postpone recursive postpone <bnf ; immediate
: ;bnf ( sys -- )  postpone bnf> postpone ; ; immediate

: @token ( -- n )   source nip >in @ = if 0
                    else source >in @ /string drop c@ then ;
: +token ( f -- )   if 1 >in +! then ;
: =token ( n -- )   success @ if @token =  dup success ! +token
                              else drop then ;
: token ( n -- )    create c, does> ( a -- )  c@ =token ;

: t"   [char] " parse 0 ?do
       dup c@ postpone literal postpone =token 1+ loop drop ; immediate
: t'   char postpone literal postpone =token ; immediate
: tok   char token ;
0 token <EOL>

variable $$
variable chainer  variable chainer1   variable chainer2  variable chainer3
: {{ postpone ahead chainer3 ! chainer2 ! chainer1 !
     postpone ; noname : latestxt chainer ! ; immediate
: }} postpone ; noname : chainer1 @ chainer2 @ chainer3 @ postpone then
     postpone $$ postpone @ postpone ,
     chainer @ postpone literal postpone , ; immediate

: execute-steps ( a a' -- a n ) here over - cell / 2/ ;
: execute-strip ( a n -- )
    0 ?do dup @ $$ ! cell+ dup @ swap >r execute r> cell+ loop drop ;
: execute-all ( a n -- ) execute-steps execute-strip ;
: <parse here 1 success ! ;
: parse> execute-all ;
