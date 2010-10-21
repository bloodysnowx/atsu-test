	.file	"examsum.c"
	.text
.globl main
	.type	main, @function
main:
	pushl	%ebp
	movl	%esp, %ebp
	subl	$16, %esp
	movl	$0, -12(%ebp)
	movl	$1, -8(%ebp)
	jmp	.L2
.L5:
	movl	$1, -4(%ebp)
	jmp	.L3
.L4:
	movl	-4(%ebp), %eax
	addl	%eax, -12(%ebp)
	addl	$1, -4(%ebp)
.L3:
	cmpl	$1000000, -4(%ebp)
	jle	.L4
	addl	$1, -8(%ebp)
.L2:
	cmpl	$100, -8(%ebp)
	jle	.L5
	leave
	ret
	.size	main, .-main
	.ident	"GCC: (Ubuntu/Linaro 4.4.4-14ubuntu5) 4.4.5"
	.section	.note.GNU-stack,"",@progbits
