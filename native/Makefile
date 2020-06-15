TARGET?=linux-x64
ifeq ($(TARGET),linux-x64)
  GCC=gcc
  CPP_FLAGS=
  LD_FLAGS=-licuuc
  EXT=so
else
  GCC=x86_64-w64-mingw32-gcc
  CPP_FLAGS=-I/opt/icu-win/include
  LD_FLAGS=-L/opt/icu-win/bin64 -licuuc64
  EXT=dll
endif

build: obj/NCoreUtils.Text.native.o
	mkdir -p dist
	$(GCC) -shared obj/NCoreUtils.Text.native.o $(LD_FLAGS) -o dist/NCoreUtils.Text.native.$(EXT)

obj/NCoreUtils.Text.native.o: src/NCoreUtils.Text.c include/NCoreUtils.Text.h .dummy
	mkdir -p obj
	$(GCC) -c -fPIC -Iinclude $(CPP_FLAGS) src/NCoreUtils.Text.c -o obj/NCoreUtils.Text.native.o

.dummy:

clean:
	rm -rf obj dist


