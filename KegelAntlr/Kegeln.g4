grammar Kegeln;

/*
 * Parser Rules
 */

calculation: expression EOF;

expression
   : multiplyingExpression (add multiplyingExpression)*
   ;
add: (PLUS | MINUS);

multiplyingExpression
   : atom (mult atom)*
   ;

atom: Number | PI | taccTuccaLand;

mult: (TIMES | DIV);

taccTuccaLand: TTL  '$' expression ;
/*
 * Lexer Rules
 */

PI: [pP][iI];
TIMES: '*';
DIV: '/';
PLUS: '+';
MINUS: '-';
TTL: 'TaccaTuccaLand ';

Number: [0-9]+ ( '.' [0-9]+ )?;

WS
	:	' ' -> channel(HIDDEN)
	;
