; dispstr.asm
	bits	16
	org		0x100
	
	mov		ah, 9
	mov		dx, msg
	int		21h
	
	mov		ah, 4Ch
	mov		al, 0
	int		21h
	
msg db		"Hello, assembler$"