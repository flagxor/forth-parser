all: expr english
	echo "SUCCESS"
	
english:
	./english1.fs -e bye
	./english2.fs

expr:
	./expr-print.fs
	./expr-eval.fs
	./expr-eval2.fs
