C_FLG = -c -DGCC_3 -D__CPP__ -Wall -O3 -I../Include

CPP_FLG = -c -DGCC_3 -D__CPP__ -Wall -O3 -I../Include

.cpp.o:
	gcc $(CPP_FLG) $<

.c.o:
	gcc $(C_FLG) $<

ALL : CS_Comp CS_DictDiff

CS_Comp : CS_COMP.o 
	gcc -v -o CS_Comp CS_COMP.o ../Source/CsMap.a -lm -lc -lgcc -lstdc++

CS_DictDiff : CS_DictDiff.o
	gcc -v -o CS_DictDiff CS_DictDiff.o ../Source/CsMap.a -lm -lc -lgcc -lstdc++


