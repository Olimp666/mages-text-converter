A command-line tool that converts text to hex and vice versa based on the sheet used to modify text in SciAdv games.
## Usage
`Converter -i *path to text to convert* -o *output file* -s *path to sheet file* -m *mode*`  
`-m` has two options: t and m. t converts text to hex, h converts hex to text.  
Sheet example:  
```
8001=0
8002=1
8003=2
8004=3
8005=4
8006=5
8007=6
8008=7
8009=8
800A=9
800B=a
800C=b
800D=c
800E=d
800F=e
8010=f
803F= 
8040=/
8041=:
8042=-
8043=;
8044=!
8045=?
8046='
8047=.
80C8=[']
80C9=[[']]
03FF=[END]
01=[NameS]
02=[NameE]
```  
Note that your text must be in UTF-8 encoding if it contains non-ASCII characters.
