# make sure you already ran pip install -r requirements.txt
#
# syntax
# pygmentize -S <style> -f <formatter> [-a <arg>] [-O <options>] [-P <option=value>]
# (pygmentize documentation is pretty scattered and confusing, but the "-a" will add other classes
# to the output)


STYLES = autumn
STYLES += borland
STYLES += bw
STYLES += colorful
STYLES += default
STYLES += emacs
STYLES += friendly
STYLES += fruity
STYLES += manni
STYLES += monokai
STYLES += murphy
STYLES += native
STYLES += pastie
STYLES += perldoc
STYLES += tango
STYLES += trac
STYLES += vim
STYLES += vs

# a recursively-expanding variable, so that its value contains an actual function call to be
# re-expanded under the control of foreach
gen_html = pygmentize -S $(style) -f html -a .highlight > $(style).css;

cssfiles:
	@$(foreach style, $(STYLES), $(gen_html))
