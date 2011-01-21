; dispchar.asm
	mov		ah, 02
	mov		dl, 31h
	int		21h
	
	mov		ah, 4Ch
	mov		al, 0
	int		21h