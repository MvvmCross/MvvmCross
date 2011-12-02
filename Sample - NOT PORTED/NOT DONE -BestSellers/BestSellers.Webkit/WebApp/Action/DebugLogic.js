var WebApp = (function() {
	var _def, _headView, _head, _header;
	var _webapp, _group, _bdo, _bdy, _file;
	var _maxw, _maxh;
	var _scrID, _scrolling, _scrAmount;
	var _opener, _radio;

	var _prev		= -1;	// for beginslide/endslide event (SlideInfo)
	var _historyPos	= -1;	// warning: must order properly var names for reduction script
	var _history	= [];
	var _loader		= [];
	var _fading		= [];
	var _ajax		= [];
	var _initialNav	= history.length;
	var _sliding	= 0;
	var _hold		= 0;
	var _baseTitle	= "";
	var _baseBack	= "";
	var _width		= 0;
	var _height		= 0;
	var _lastScroll	= 1;
	var _dialog		= null;
	var _scrolled	= 1;
	var _proxy		= "";
	var _pil		= 0;
	var _tmp		= setInterval(InitBlocks, 250);
	var _locker		= null;
	var _win		= window;
	
	// RFC 2397 (http://www.scalora.org/projects/uriencoder/)
	var _blank		= "data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==";

	var _wkt;
	var _v2			= !!document.getElementsByClassName && UA("WebKit");	// FIXME: no UA?
	var _fullscreen	= !!navigator.standalone;
	var _touch		= IsDefined(_win.ontouchstart) && !UA("Android");	// FIXME: null on android???
	var _translator	= _touch ? tr_iphone : tr_others;					// FIXME: added for WKT bug on Iphone 3

	var _handler	= {};
	_handler.load				= [];
	_handler.beginslide			= [];
	_handler.endslide			= [];
	_handler.beginasync			= [];
	_handler.willasync			= [];
	_handler.endasync			= [];
	_handler.orientationchange	= [];
	_handler.tabchange			= [];

/* Public */
	var _o_acl = false;

	var d = document;
	var $h = {
		get HEAD() { return 0 },
		get BACK() { return 1 },
		get HOME() { return 2 },
		get LEFT() { return 3 },
		get RIGHT() { return 4 },
		get TITLE() { return 5 }
	};
	var $d = {
		get L2R() { return +1 },
		get R2L() { return -1 }
	};
	d.wa = {
		get autoCreateLayer() { return _o_acl; },
		set autoCreateLayer(v) { _o_acl = (v == "true" || v == "yes" || v === true) },
		
		get header() { return $h },
		get direction() { return $d }
	};
	d.webapp = d.wa;

	var $pc = {
		get Version() { return 'v0.5.2' },

		Proxy: function(url) { _proxy = url },

		Progressive: function(enable) { _pil = enable },	// TODO: xml based selection + parent stack

		Opener: function(func) {
			_opener = func ? func : function(u) { location = u };
		},

		Refresh: function(id) {
			if (id !== false) {
				var o = $(id);

				if (!o) {
					InitForms();
				} else if (o.type == "radio") {
					UpdateRadio([o]);
				} else if (o.type == "checkbox") {
					FlipCheck(o.previousSibling, 1);
				}
			}

			/* reset layer title */
			SetTitle();
			SetBackButton();

			/* force adjustment in height of the active layer */
			Resizer();
		},

		HideBar: function() {
			if (_scrolled && IsMobile()) {
				_scrolled = 0;
				ToTop(1); setTimeout(ToTop, 0);
			}
			return false;
		},

		Header: function(show, what, keep) {
			ShowHeader();			// HACK: be sure to always have all header elements displayed before removing them...
			if (keep != -1)	{		// keep all header controls?
				Buttons(!show, keep);
			}

			Display(_headView, 0);	// hide previous if any
			_headView = $(what);
			Display(_headView, show);

			_header[$h.HEAD].style.zIndex = !show ? 2 : "";
			return false;
		},

		Tab: function(id, active) {
			var o = $(id);
			ShowTab(o, $$("li", o)[active]);
		},

		AddEventListener: function(evt, handler) {
			if (IsDefined(_handler[evt])) {
				with (_handler[evt]) {
					if (indexOf(handler) == -1) {
						push(handler);
					}
				}
			}
		},

		RemoveEventListener: function(evt, handler) {
			if (IsDefined(_handler[evt])) {
				with (_handler[evt]) {
					splice(lastIndexOf(handler), 1);
				}
			}
		},

		Back: function() {
			if (_hold) {
				return (_hold = 0);
			}
			_radio = null;
			if (history.length - _initialNav == _historyPos) {
				history.back();
			} else {
				_opener(_history[_historyPos - 1][1]);
			}
			return false;
		},

		Home: function() {
			if (history.length - _initialNav == _historyPos) {
				history.go(-_historyPos);
			} else {
				_opener("#");
			}
			return (_hold = 0);
		},

		Form: function(frm, focus) {
			var s, a, b, c, o, k, f, t, i;

			a = $(frm);
			b = $(_history[_historyPos][0]);
			s = (a.style.display != "block");

			f = GetForm(a);
			with (_header[$h.HEAD]) {
				t = offsetTop + offsetHeight;
			}

			// WARNING: this variable (k?) must not be changed, else onsumbit will not point to the function anymore!
			if (s) { a.style.top = t + "px"; }
			if (f) {
				k = f.onsubmit;
				if (!s) {
					f.onsubmit = f.onsubmit(0, 1);
				} else {
					f.onsubmit = function(e, b) {
						if (b) { return k; }			// return the old onsubmit
						if (e) { NoEvent(e); }			// or process actions...
						if (!(k && k(e) === false)) {
							$pc.Submit(this, null, e);
						}
					};
				}
			}

			Display(a, s);
			Shadow(s, t + a.offsetHeight);

			o = $$("legend", a)[0];
			SetTitle(s && o ? o.innerHTML : null);

			_dialog = (s) ? a : null;
			if (s) { c = a; a = b; b = c }

			DelLayerButtons(a);
			AddLayerButtons(b, s);

			if (s) {
				$pc.Header();
				if (focus && (i = $$("input", f)[0])) {
					i.focus();
				}
			} else {
				Buttons(!s);
			}
			return false;
		},

		Submit: function(frm) {
			var f = GetForm(frm);
			if (f) {
				var a = arguments[1];
				var _ = function(i, f) {
					var q = "";
					for (var n = 0; n < i.length; n++) {
						i[n].blur();
						if (i[n].name && !i[n].disabled && (f ? f(i[n]) : 1)) {
							q += "&" + i[n].name + "=" + encodeURIComponent(i[n].value);
						}
					}
					return q;
				}

				var q  = _($$("input", f),
					function(i) {
						with(i) {
							return ((Contains(type, ["text", "password", "hidden", "search"]) ||
									(Contains(type, ["radio", "checkbox"]) && checked)))
						}
					});
					q += _($$("select", f));
					q += _($$("textarea", f));
					q += "&" + (a && a.id ? a.id : "__submit") + "=1";
					q  = q.substr(1); // TODO: add q to action if method=get (use AddParam)
				
				/*	Use getAttribute instead of f.action because if an element of the form
					is named "action", the element will be returned instead of the action attr.
				 */
				var u = ($A(f, "action") || self.location.href);
				if ($A(f, "method").toLowerCase() != "post") {
					u = AddParams(u, q);
					q = null;
				}

				BasicAsync(u, null, q);
				if (_dialog) { $pc.Form(_dialog); }
			}

			return false;
		},

		Postable: function(keys, values) {
			var q = "";
			for (var i = 1; i < values.length && i <= keys.length; i++) {
				q += "&" + keys[i - 1] + "=" + encodeURIComponent(values[i]);
			}
			return q.replace(/&=/g, "&").substr(1);
		},

		Request: function(url, prms, cb, async, loader) {
			// allow default callback only when document is fully loaded
			if (_historyPos === cb) {
				return;
			}

			var r, a = [url, prms];
			if (!CallListeners("beginasync", a)) {
				if (loader) {
					setTimeout(DelClass, 100, loader, "__sel");	// TO have feedback - will be removed if touchstart is added
				}
			} else {
				url		= a[0];	// Can be changed in beginasync event
				prms	= a[1];
	
				cb = cb == -1 ? DefaultCallback() : cb;
	
				var o = new XMLHttpRequest();
				var c = function() { __callback(o, cb, loader) };
				var m = prms ? "POST" : "GET";
	
				async = !!async;
				if (loader) { $pc.Loader(loader, 1); }
				_ajax.push([o, a]);
	
				url = AddParam(url, "__async", "true");
				if (_historyPos >= 0) {
					url = AddParam(url, "__source", _history[_historyPos][0]);
				}
				url = SetURL(url);
	
				o.open(m, url, async);
				if (prms) { o.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); }
				CallListeners("willasync", a, o);
				o.onreadystatechange = (async) ? c : null;
				o.send(prms);
	
				if (!async) { c(); }
			}
		},

		Loader: function(obj, show) {
			var o, h, f;

			if (o = $(obj)) {
				h = HasClass(o, "__lod");
				ApplyMore(o);
	
				if (show) {
					// If already running remove it to make the new loader image work if style has change
					if (h) { $pc.Loader(obj, 0); }
					AddClass(o, "__lod");
					_loader.push([o, AnimPrepare(o)]);
	
				} else if (h) {
					DelClass(o, "__lod");
					f = _loader.filter(function(f) { return f[0] == o })[0];
					Remove(_loader, f);
					if (f = f[1]) {
						f[0]._ = 0;
						clearInterval(f[1]);
						f[0].style.backgroundImage = "";
					}
				}
			}
			return h;
		},

		Player: function(src) {
			if (!IsMobile()) {
				_win.open(src);
			} else {

				// prevent the back request sent by the internal player
				if (_v2) { location = "#" + Math.random(); }

				var w = $("__wa_media");
				var o = NewItem("iframe");
				o.id = "__wa_media";
				o.src = src;			// must be before appendChild to prevent Safari weird behavior

				_webapp.appendChild(o);
				DelItem(w);
			}
			return false;
		},

		toString: function() { return "[WebApp.Net Framework]"; }
	}

	function ToTop(h) {
		h = h ? h : 0;
		_webapp.style.minHeight = (_height + h) + "px";
		_win.scrollTo(0, h);
	}

	function CalcEaseOut(s, w, dir, step, mn) {
		s += Math.max((w - s) / step, mn || 4);
		return [s, (w + w * dir) / 2 - Math.min(s, w) * dir];
	}

	function ApplyMore(o) {
		if (HasClass(o, "iMore")) {
			var a = $$("a", o)[0];
			if (a && a.title) {
				var s = $$("span", a)[0] || a;
				o = s.innerHTML;
				s.innerHTML = a.title;
				a.title = o;
			}
		}
	}

	function Buttons(s, k) {
		if (_head) {
			var h = _header;
			k = (s) ? [] : k || []; // if have to show header, ignore k parameter

			for (var i = 1; i < h.length; i++) {	// don't hide HEAD, just hide content
				if (!Contains(i, k)) {
					Display(h[i], s);
				}
			}

			with ($h) {
				if (!Contains(BACK, k)) {
					Display(h[BACK], s && !h[LEFT] && _historyPos);
				}
				if (!Contains(HOME, k)) {
					Display(h[HOME], s && !h[RIGHT] && !_hold && _historyPos > 1);
				}
			}
		}
	}

	function AddLayerButtons(lay, ignore) {
		if (_head) {
			var a = $$("a", lay);
			var p = $h.RIGHT;
	
			for (var i = 0; i < a.length && p >= $h.LEFT; i++) {
				if (_header[p] && !ignore) { i--; p--; continue; }

				if (HasToken(a[i].rel, "action") ||
					HasToken(a[i].rel, "back")) {
	
					AddClass(a[i], p == $h.RIGHT ? "iRightButton" : "iLeftButton");
					Display(a[i], 1);
					_header[p--] = a[i];
					_head.appendChild(a[i--]);
				}
			}
		}
	}

	function DelLayerButtons(lay) {
		if (_head) { 
			with ($h) {
				for (var i = LEFT; i <= RIGHT; i++) {
					var a = _header[i];
					if (a && (	HasToken(a.rel, "action") ||
								HasToken(a.rel, "back")) ) {
		
						Display(a, 0);
						DelClass(a, i == RIGHT ? "iRightButton" : "iLeftButton");
						lay.insertBefore(a, lay.firstChild);
					}
				}
				_header[RIGHT] = $("waRightButton");
				_header[LEFT] = $("waLeftButton");
			}
		}
	}

	function AnimData(o) {
		var u;
/*
	[1] = first part of the filename
	[2] = delay
	[3] = frame count
	[4] = frame index
	[5] = end part of the filename
*/
		if (u = getComputedStyle(o).backgroundImage) {
			o._ = 1;	// To track if loading animation is still active
			return /(.+?(\d+)x(\d+)x)(\d+)(.*)/.exec(u);
		}
	}

	function AnimPrepare(o) {
		var d, c, i;
		
		// if the object has no background with anim definition, search any child
		if (!(d = AnimData(o))) {
			c = $$("*", o);
			for (i = 0; i < c.length; i++) {
				o = c[i];
				if (d = AnimData(o)) {
					break;
				}
			}
		}
		// return the animator or nothing
		return (d) ? [o, setInterval(AnimRun, d[2], [o, d[4], d[3], (d[1] + "*" + d[5]), new Image() ])] : d;
	}

	function AnimRun(a) {
		if (!a[5]) {
			a[1] = parseInt(a[1]) % parseInt(a[2]) + 1;
			var b = a[3].replace("*", a[1]);
			a[4].onload = function() { if (a[0]._) a[0].style.backgroundImage = b; a[5] = 0 };
			a[5] = a[4].src = b.substr(4, b.length - 5);
		}
	}

/* Private */
	function NoTag(s) { return s.replace(/<.+?>/g, "").replace(/^\s+|\s+$/g, "").replace(/\s{2,}/, " "); }
	function NoEvent(e) { e.preventDefault(); e.stopPropagation(); }
	function IsAsync(o) { return HasToken(o.rev, "async") || HasToken(o.rev, "async:np"); }
	function IsMedia(o) { return HasToken(o.rev, "media") /*|| HasToken(o.rev, "media:audio") || HasToken(o.rev, "media:video");*/ }
	function IsDefined(o) { return (typeof o != "undefined"); }
	function Contains(o, a) { return a.indexOf(o) != -1 }

	function $(i) { return typeof i == "string" ? document.getElementById(i) : i; }
	function $$(t, o) { return (o || document).getElementsByTagName(t); }
	function $A(o, a) { return o.getAttribute(a) || ""; }

	function XY(e) {
		var x = 0;
		var y = 0;

		while (e) {
			x += e.offsetLeft;
			y += e.offsetTop;
			e  = e.offsetParent;
		}

		return {x:x, y:y};
	}
	
	function WIN() {
		with (_win) return { x:pageXOffset, y:pageYOffset, w:innerWidth, h:innerHeight };
	}

	function NewScript(c) {
		var	s, h = $$("head")[0];
		s = NewItem("script");
		s.type = "text/javascript"; // should be application/x-javascript see RFC4329
		s.textContent = c;
		h.appendChild(s);
	}
	
	function NewItem(t, c) {
		var o = document.createElement(t);
		if (c) { o.innerHTML = c; }
		return o;
	}
	function DelItem(p, c)	{
		if (p) {
			if (!c) {
				c = p;
				p = c.parentNode;
			}
			p.removeChild(c);
		}
	}
	function GetForm(o)	{
		o = $(o);
		if (o && GetName(o) != "form") {
			o = GetParent(o, "form");
		}
		return o;
	}
	function GetLink(o)		{ return GetName(o) == "a" ? o : GetParent(o, "a") }
	function GetName(o)		{ return o.localName.toLowerCase() }
	function HasToken(o, t)	{ return o && Contains(t, o.toLowerCase().split(" ")); }

	function HasClass(o, c)	{ return o && Contains(c, GetClass(o)); }
	function GetClass(o)	{ return o.className.split(" "); }
	function AddClass(o, c) {
		var h = HasClass(o, c);
		if (!h) { o.className += " " + c; }
		return h;
	}
	function DelClass(o) {
		var c = GetClass(o);
		var a = arguments;
		for (var i = 1; i < a.length; i++) {
			Remove(c, a[i]);
		}
		o.className = c.join(" ");
	}
	function GetParent(o, t) {
		while ((o = o.parentNode) && (o.nodeType != 1 || GetName(o) != t)){};
		return o;
	}
	function AnyOf(o, c) {
		while ((o = o.parentNode) && (o.nodeType != 1 || !HasClass(o, c))){};
		return o;
	}

	function Remove(a, e) {
		var p = a.indexOf(e);
		if (p != -1) { a.splice(p, 1); }
	}

	function GetText(o) {
		o = o.childNodes;
		for (var i = 0; i < o.length; i++) {
			if (o[i].nodeType == 3) {
				return o[i].nodeValue.replace(/^\s+|\s+$/g, "");
			}
		}
		return null;
	}

	function InitBlocks() {
		if (!_webapp)	{ _webapp	= $("WebApp"); }
		if (!_group)	{ _group	= $("iGroup"); }

		var i = $("iLoader");
		if (i && !HasClass(i, "__lod")) {
			$pc.Loader(i, 1);
		}
	}

	function InitVars() {
		_header = [
			$("iHeader"),
			$("waBackButton"),
			$("waHomeButton"),
			$("waLeftButton"),
			$("waRightButton"),
			$("waHeadTitle")
		];

		_bdy = document.body;
		_bdo = (_bdy.dir == "rtl") ? -1 : +1;
		_wkt = IsDefined(_bdy.style.webkitTransform);		
	}

	function Display(o, s) { if (o = $(o)) { o.style.display = s ? "block" : "none"; } }
	function Layer(o, s) {
		if (o = $(o)) {
			o.style[_bdo == 1 ? "left" : "right"] = s ? 0 : "";
			o.style.display = s ? "block" : "";
		}
	}

	// TODO: any way to do this with CSS??
	function AdjustLayer(o) {
		if (o = o || GetActive()) {
			var z = $$("div", o); z = z[z.length -1];
			if (z && (HasClass(z, "iList") || HasClass(z, "iFull"))) {
				z.style.minHeight = parseInt(_webapp.style.minHeight) - XY(z).y + "px";
			}
		}
	}

	function Shadow(s, p) {
		var o = $("__wa_shadow");
		o.style.top = p + "px";
		// display:relative may slow down webkit effect, use it only when required
		_webapp.style.position = s ? "relative" : "";
		Display(o, s);
	}

	function Historize(o, l) {	// l = isDefault
		if (o) {
			//TODO: fix endless toggle when using same layer in different level of navigation
			
			_history.splice(++_historyPos, _history.length);
			_history.push([o, !l ? location.hash : ("#_" + _def.substr(2)), _lastScroll]);

		}
	}

	// We need to remove scripts from the clone to prevent execution
	function PrepareClone(o) {
		// prevent execution of script tag
		var s = $$("script", o);
		while(s.length) {
			DelItem(s[0]);
		}
		return o;
	}

	function Cleanup() {
		var s, i, c;

// FIXME: may cancel some unwanted visual loaders

		while (_loader.length) {
			$pc.Loader(_loader[0][0], 0);
		}
		s = $$("li");
		for (i = 0; i < s.length; i++) {
			DelClass(s[i], "__sel", "__tap");
		}
	}

	function ParseParams(s, np) {
		var ed = s.indexOf("#_");
		if (ed == -1) {
			return null;
		}
		var rs = "";
		var bs = SplitURL(s);
		if (!np) {
			for (var i = 0; i < bs[1].length; i++) {
				rs += "/" + bs[1][i].split("=").pop();
			}
		}
		return bs[2] + rs;
	}

	function IsMobile() {
		return (UA("iPhone") || UA("iPod") || UA("Aspen"));
	}

	function UA(s) {
		return Contains(s, navigator.userAgent);
	}

	function Resizer() {
		if (_sliding) {
			return;
		}
		var m, h, o, w = (WIN().w >= _maxh) ? _maxh : _maxw;
		if (w != _width) {
			_width = w;
			_webapp.className = (w == _maxw) ? "portrait" : "landscape";
			CallListeners("orientationchange");
		}

		if (o = GetActive()) {
			h = XY(o).y + o.offsetHeight;
		}

		m = _width == _maxw ? 416 : 268;	// minimum height values for Safari iPhone
		w = WIN().h;
		h = h < w ? w : h;
		h = h < m ? m : h;

		_height = h;
		_webapp.style.minHeight = h + "px";
		AdjustLayer();

	}

	function Locator() {
		if (_sliding || _hold == location.href) {
			return;
		}
		_hold = 0;

		var act = GetActive();
		if (act) {
			act = act.id;
		} else if (location.hash.length > 0) {	// there is a simple anchor, jump to it
			return;
		} else {						// No? should slide back to home
			act = _history[0][0];
		}

		var cur = _history[_historyPos][0];
		if (act != cur) {
			var i, pos = -1;
			for (i in _history) {
				if (_history[i][0] == act) {
					pos = parseInt(i);
					break;
				}
			}
			if (pos != -1 && pos < _historyPos) {
// TODO: check this, seems it is useless and make URL based change to an invalid history position, should fix back madness
// Locator redefini _historyPos ? = erreur de position quand # (_historyPos = pos + 1)
//				_historyPos = pos + 1;
				InitSlide(cur, act, $d.L2R);
			} else {
				SlideTo(act);
			}
		}
	}

	function CallListeners(evt, ctx, obj) {
		// Do not waste time and memory if no handler have been defined for the given event
		var l = _handler[evt].length;
		if (l == 0) {
			return true;
		}
		var e = {
			type: evt,
			target: obj || null,
			context: ctx || Explode(_history[_historyPos][1]),
			windowWidth: _width,
			windowHeight: _height 
		}

		var k = true;
		for (var i = 0; i < l; i++) {
			k = k && (_handler[evt][i](e) == false ? false : true);
		}
		return k;
	}

	function Init() {

		clearInterval(_tmp);

		InitBlocks();
		InitVars();
		InitForms();
		InitTab();
		InitHeader();
		InitObj("__wa_shadow");

		var i = $("iLoader");
		$pc.Loader(i, 0);
		DelItem(i);

		$pc.Opener(_opener);

		// get screen size
		_maxw = screen.width;
		_maxh = screen.height;
		if (_maxw > _maxh) { var l = _maxh; _maxh = _maxw; _maxw = l; }

		// Get the default layer
		_def = GetLayers()[0].id;
		Historize(_def, 1);

		var a = (GetActive() || "").id;
		if (a != _def)	{ Historize(a); }				// FIXME: should historize extra params too if any (?)
		if (!a)			{ a = _def; _opener("#"); }

		Layer(a, 1);
		AddLayerButtons($(a));

		with ($h) {
			var h = _header;
			Display(h[BACK], (!h[LEFT] && _historyPos));
			Display(h[HOME], (!h[RIGHT] && _historyPos > 1 && a != _def));
	
			if (h[BACK]) {
				_baseBack = h[BACK].innerHTML;
			}
			if (h[TITLE]) {
				_baseTitle = h[TITLE].innerHTML;
				SetTitle();
			}
		}

		/*	start common jobs
		*/
		setInterval(Locator, 250);

		/*	call load listener in blocking mode to allow loading of destination layer
		*/
		CallListeners("load");

		_webapp.addEventListener("touchstart", new Function(), false);	// active state
		(_touch ? _group : document).addEventListener(_touch ? "touchmove" : "scroll", ImagesListener, false);

		Resizer();
		ImagesShow();
		DocumentTracker("DOMSubtreeModified");
		DocumentTracker("resize");
		$pc.HideBar();
	}

/* Event */
	function ShowTab(ul, li, h, ev) {	// TODO: load async content here?
		if (!(	HasClass(li, "__dis") ||
				HasToken($$("a", li)[0].rel, "action"))) {

			var c, s, al = $$("li", ul);
			for (var i = 0; i < al.length; i++) {
				c = (al[i] == li);
				if (c) { s = i; }						// check which has been selected
	
				Display(ul.id + i, (!h && c));	// display/hide the panel if no override (h)
				DelClass(al[i], "__act");			// unselected any tabs
			}

			AddClass(li, "__act");
			if (ev) { CallListeners("tabchange", [s], ul); }
		}
	}

	function ClearTransform(o) {
		if (o) o.style.webkitTransform = "";
	}

	function ListenClick(e) {
		if (_sliding) {
			return NoEvent(e);
		}
		/* Checkbox label */
		var o = e.target;
		var n = GetName(o);
		if (n == "label") {
			var f = $($A(o, "for"));
			if (HasClass(f, "iToggle")) {
				setTimeout(FlipCheck, 1, f.previousSibling, 1);
			}
			return;
		}

		/* Radio parent */
		var li = GetParent(o, "li");		
		if (li && HasClass(li, "iRadio")) {
			AddClass(li, "__sel");
			ShowRadio(li);
			_hold = location.href;
			SlideTo("wa__radio");
			return NoEvent(e);
		}

		/* handle onclick event on links */
		var a = GetLink(o);
		if (a && li && HasClass(li, "__dis")) {
			return NoEvent(e);
		}
		/*	Warning: if onclick="return false" and do not call any other code,
			a.onclick will be null and the following code will not be executed.
		*/
        if (a && a.onclick) {
			var old = a.onclick;
			a.onclick = null;			// prevent double execution
			var val = old.call(a, e);
			setTimeout(function() { a.onclick = old }, 0);
			if (val === false) {
				if (li) {
					AddClass(li, HasClass(a, "iSide") ? "__tap" : "__sel");
					Unselect(li);
				}
				return NoEvent(e);
			}
        }

		/* Tab */
		var ul = GetParent(o, "ul");
		var pr = !ul ? null : ul.parentNode;
		var ax = a && IsAsync(a);

		if (a && ul && HasClass(pr, "iTab")) {
			var h, t;

			t = HasToken(a.rel, "action");	// allows classic link on tab
			h = $(ul.id + "-loader");
			Display(h, 0);

			if (!t && ax) {
				Display(h, 1);
				$pc.Loader(h, 1);
				BasicAsync(a, function(o) {
					Display(h, 0);
					$pc.Loader(h, 0);
					Display(ShowAsync(o)[0], 1);
					ShowTab(ul, li, 0, 1);
				});
			} else { h = t }				// activation only if loader doesn't exists or disabled (!ax)
			ShowTab(ul, li, !!h, !ax);		// !ax = event will be raised by async callback

			if (!t) { return NoEvent(e); }	// will be processed as classic link
		}

		/* Common button */
		if (a && Contains(a.id, ["waBackButton", "waHomeButton"])) {
			if (a.id == "waBackButton")	{ $pc.Back(); }
			else						{ $pc.Home(); }
			return NoEvent(e);
		}

		/* Radio list */
		if (ul && HasClass(ul, "iCheck")) {
			if (ClickRadio(a, ul) !== false) {
				var al = $$("li", ul);
				for (var i = 0; i < al.length; i++) {
					DelClass(al[i], "__act", "__sel");
				}
				AddClass(li, "__act __sel");
				setTimeout(DelClass, 1000, li, "__sel");
			}
			return NoEvent(e);
		}

		/* Menu and list */
		if (ul && !HasClass(li, "iMore") &&
			((HasClass(ul, "iMenu") || HasClass(pr, "iMenu")) ||
			 (HasClass(ul, "iList") || HasClass(pr, "iList"))) ) {

			if (a && !HasClass(a, "iButton")) {
				var c = AddClass(li, HasClass(a, "iSide") ? "__tap" : "__sel");
				if (ax) {
					if (!c) { BasicAsync(a); }
					return NoEvent(e);
				}
			}
		}

		/* More */
		var dv = AnyOf(o, "iMore");
		if (dv) {
			if (!HasClass(dv, "__lod")) {
				$pc.Loader(dv, 1);
				if (ax) { BasicAsync(a); }
			}
			return NoEvent(e);
		}

		/* Top form button */
		if (a && _dialog) {
			if (HasToken(a.rel, "back")) {
				$pc.Form(_dialog, a);
				return NoEvent(e);
			}
			if (HasToken(a.rel, "action")) {
				var f = GetForm(_dialog);
				if (f) {
					f.onsubmit(e);
					return;
				}
			}
		}

		/* Media player */
		if (a && IsMedia(a)) {
			Unselect(li);
			/*if (!d)*/ $pc.Player(a.href, a);
			return NoEvent(e);
		}

		/* Basic async link */
		if (ax) {
			BasicAsync(a);
			NoEvent(e);

		} else if (a && !a.target) {
			/* Basic go layer */
			if (startsWith(a.href, "http:", "https:", "file:")) {	// file: for local testing
				Forward(a.href);
				NoEvent(e);
			}
			Unselect(li);
		}
	}

	function Unselect(li) {
		if (li) { setTimeout(DelClass, 1000, li, "__sel", "__tap"); }
	}

	function startsWith(s1) {
		var r, i, a = arguments;
		for (i = 1; i < a.length; i++) {
			if (s1.toLowerCase().indexOf(a[i]) == 0) {
				return 1;
			}
		}
	}

/* Animate */

	function SlideTo(to) {
		var h = _history[_historyPos][0];
		if (h != to) {
			InitSlide(h, to);
		}
	}

	function InitSlide(src, dst, dir) {
		if (_sliding) {
			return;
		}
		_sliding = 1;

		AdjustView();
		if (dst == _history[0][0]) {
			_initialNav = history.length;
		}
		dir = dir || $d.R2L;
		src = $(src);
		dst = $(dst);

		var h;

		if (_wkt && _head) {
			h = PrepareClone(_head.cloneNode(true));
		}

		_prev = _historyPos;
		if (dir == $d.R2L) {
			Historize(dst.id);
		} else {
			while (_historyPos && _history[--_historyPos][0] != dst.id){};
		}

// New title bar
		HideHeader();
		DelLayerButtons(src);
		AddLayerButtons(dst);
		ShowHeader();

		if (h) { _header[$h.HEAD].appendChild(h); }
		SetBackButton((dir != $d.R2L) ? "" : (_hold ? "" : NoTag(src.title)) || _baseBack);

		SetTitle(_hold ? dst.title : null);
		DoSlide(src, dst, dir);
	}

	function SetBackButton(txt) {
		if (_header[$h.BACK]) {
			if (!txt && _historyPos) {
				txt = NoTag($(_history[_historyPos - 1][0]).title) || _baseBack;
			}
			if (txt) { _header[$h.BACK].innerHTML = txt; }
		}
	}

	function SlideInfo(m) {
		var s = Explode(_history[_prev][1]);
		var d = Explode(_history[_historyPos][1]);
		var r = (m < 0 && !!_hold) ? ["wa__radio"] : d;
		return [s, d, m, r];
	}
	
	function tr_iphone(t) { return "translate3d(" + t + ",0,0)"; }
	function tr_others(t) { return "translateX(" + t + ")"; }

	function TranslateX(o, t, i) {
		if (o) {
			if (t) { t = _translator(t); }
			o.style.webkitTransitionProperty = (i) ? "none" : "";
			o.style.webkitTransform = t;
		}
	}	

	function GetTiming(o) {
		return o ? getComputedStyle(o, null).webkitTransitionDuration : "0s";
	}
	
	function GetHigherOf() {
		var r, t, i, j, a = arguments;		
		r = 0;
		for (i = 0; i < a.length; i++) {
			t = GetTiming(a[i]).split(',');
			for (j = 0; j < t.length; j++) {
				r = Math.max(r, parseFloat(t[j]) * 1000);
			}
		}
		return r;
	}

	function DoSlide(src, dst, dir) {
		CallListeners("beginslide", SlideInfo(dir));

		InitForms(dst);
		Layer(src, 1);
		Layer(dst, 1);

		// default effect if not webkit
		if (!_wkt) {
			EndSlide(src, dst, dir);
			return;
		}

		var b = _group;
		var w = _webapp;
		var g = dir * _bdo;

		// set the height of iGroup to match the real height of the effect
		b.style.height = (_height - b.offsetTop) + "px";

		// layer anim
		AddClass(w, "__ani");
		TranslateX(src, "0", 1);
		TranslateX(dst, (g * -100) + "%", 1);

		// header anim
		var h, hcs, hos, tim = GetHigherOf(src, dst, _head, _header[$h.TITLE]);
		if (_head) {
			h = _header[$h.HEAD].lastChild;
			hcs = h.style;
			hos = _head.style;

			hcs.opacity = 1;
			hos.opacity = 0;
			TranslateX(h, "0", 1);
			TranslateX(_head, (g * -20) + "%", 1);
			TranslateX(_header[$h.TITLE], (g == $d.R2L ? 60 : -20) + "%", 1);
		}

		setTimeout(function() {
			AdjustLayer(dst);

			TranslateX(dst, "0");
			TranslateX(src, (g * 100) + "%");

			if (h) {
				hcs.opacity = 0;
				hos.opacity = 1;
				TranslateX(h, (g * 30) + "%");
				TranslateX(_head, "0");
				TranslateX(_header[$h.TITLE], "0");
			}

			setTimeout(function() {
				if (h) { DelItem(_header[$h.HEAD], h); }
				DelClass(w, "__ani");
				b.style.height = "";

				EndSlide(src, dst, dir);
			}, tim);
		}, 0);
	}

	function EndSlide(src, dst, dir) {
		Cleanup();
		Layer(src, 0);

		if (_wkt) {
			ClearTransform(dst);
			ClearTransform(src);
			ClearTransform(_head);
			ClearTransform(_header[$h.TITLE]);
		}

		CallListeners("endslide", SlideInfo(dir));

		_sliding = 0;
		_prev = -1;

		Resizer();
		setTimeout(AdjustView, 0, dir == $d.L2R ? _history[_historyPos + 1][2] : null);
		setTimeout(ImagesShow, 0);
	}

	function SetTitle(title) {
		var o;
		if (o = _header[$h.TITLE]) {
			o.innerHTML = title || GetTitle(GetActive()) || _baseTitle;
		}
	}

	function HideHeader() {
		if (_dialog) { $pc.Form(_dialog); }
		Display(_headView, 0);
	}

	function ShowHeader() {
		Buttons(1);
	}

// <b><b>OFF</b><i></i></b>
//     o
//  o

	function FlipCheck(o, dontChange) {
		var c = o, i = $(c.title);
		var txt = i.title.split("|");

		if (!dontChange) {
			i.click();
		}
		(i.disabled ? AddClass : DelClass)(c, "__dis");

		o = c.firstChild.nextSibling;
		with (c.lastChild) {
			innerHTML = txt[i.checked ? 0 : 1];
			if (i.checked) {
				o.style.left = "";
				o.style.right = "-1px";
				AddClass(c, "__sel");
				style.left = 0;
				style.right = "";
			} else {
				o.style.left = "-1px";
				o.style.right = "";
				DelClass(c, "__sel");
				style.left = "";
				style.right = 0;
			}
		}
	}

	function AdjustView(to) {
		var h = to ? to : Math.min(50, WIN().y);
		var s = to ? Math.max(1, to - 50) : 1;
		var d = to ? -1 : +1;

		while (s <= h) {
			var z = CalcEaseOut(s, h, d, 6, 2);
			s = z[0]; _win.scrollTo(0, z[1]);
		}
		if (!to) { $pc.HideBar(); }
	}

	function Explode(loc) {
		// WARNING: with classic anchors the returned value of this function will be wrong
		if (loc) {
			var p = loc.indexOf("#_");
			
			if (p != -1) {
				loc = loc.substring(p + 2).split("/");
				var id = "wa" + loc[0];
				for (var i in loc) {
					loc[i] = decodeURIComponent(loc[i]);
				}
				loc[0] = id;

				if (_o_acl && !$(id)) {
					CreateLayer(id);
				}
				return $(id) ? loc : [];
			}
		}
		return [];
	}

	function GetLayers() {
		var lay = [];
		var src = _group.childNodes;
		for (var i in src) {
			if (src[i].nodeType == 1 && HasClass(src[i], "iLayer")) {
				lay.push(src[i]);
			}
		}
		return lay;
	}
	
	function CreateLayer(i) {
		var n = NewItem("div");
		n.id = i;
		n.className = "iLayer";
		_group.appendChild(n);
		return n;
	}

	function GetTitle(o) {
		return (!_historyPos && _baseTitle) ? _baseTitle : o.title;
	}

	function GetActive() {
// FIXME: should always return the active layer even if the hash is incorrect or use a classic anchor?
		var h = location.hash;
		return $(!h ? _def : Explode(h)[0]);
	}

	function SetURL(url) {
		var d = url.match(/[a-z]+:\/\/(.+:.*@)?([a-z0-9-\.]+)((:\d+)?\/.*)?/i);
		return (!_proxy || !d || d[2] == location.hostname)
			? url : AddParam(_proxy, "__url", url);	// FIXME??? was __url=?
	}

	function SplitURL(u) {
		var s, q, d;

		s = u.replace(/&amp;/g, "&");
		d = s.indexOf("#");
		d = s.substr(d != -1 ? d : s.length);
		s = s.substr(0, s.length - d.length);
		q = s.indexOf("?");
		q = s.substr(q != -1 ? q : s.length);
		s = s.substr(0, s.length - q.length);
		q = !q ? [] : q.substr(1).split("&");

		return [s, q, d];
	}

	function AddParam(u, k, v) {
		u = SplitURL(u);
		var q = u[1].filter(
				function(o) { return o && o.indexOf(k + "=") != 0 });	// != 0 => any parameter not starting with (k + "=") no multiple name allowed!!! FIXME?
		q.push(k + "=" + encodeURIComponent(v));
		return u[0] + "?" + q.join("&") + u[2];
	}
	
	function AddParams(u, q) {
		u = SplitURL(u);
		u[1].push(q);

		return u[0] + "?" + u[1].join("&") + u[2];
	}

	function BasicAsync(item, cb, q) {
		var h, o, u, i;

		i = (typeof item == "object");
		u = (i ? item.href : item);
		o = GetParent(item, "li");	// get loader

		if (!cb) { cb = DefaultCallback(u, HasToken(item.rev, "async:np")); }
		$pc.Request(u, q, cb, true, o, (i ? item : null));
	}


// TODO: optimize this!!!!
	function DefaultCallback(i, np) {
		return function(o) {
			var u = i ? ParseParams(i, np) : null;
			var g = ShowAsync(o);

			if (g && (g[1] || u)) {
				Forward(g[1] || u);
			} else {
				Cleanup(); //setTimeout(Cleanup, 250);
			}
			return null;
		};
	}
	
	function ReadTextNodes(o) {
		var nds = o.childNodes;
		var txt = "";
		for (var y = 0; y < nds.length; y++) {
			txt += nds[y].nodeValue;
		}
		return txt;
	}
	
	function Forward(l) {
		_lastScroll = WIN().y;
		AdjustView();
		_opener(l);
	}
	
	function Go(g) {
		return "#_" + g.substr(2);
	}

	function ResetScroll(i) {
		if (i.substr(0, 2) == "wa") {
			var p = _historyPos;

			if (p && i == _history[0][0]) {
				_history[1][2] = 0;
			}
			while (p && _history[--p][0] != i){};
			if (p) { _history[p + 1][2] = 0; }
		}
	}

	function ShowAsync(o) {
		if (o.responseXML) {
			o = o.responseXML.documentElement;

			var s, t, k, a = GetActive();

			/* force jump to a given layer */
			var g = $$("go", o);
			g = (g.length != 1) ? null : $A(g[0], "to");

			/* get all parts to update */
			var f, p = $$("part", o);
			if (p.length == 0) { p = [o]; }

			for (var z = 0; z < p.length; z++) {
				var dst = $$("destination", p[z])[0];
				if (!dst) { break; }

				var mod = $A(dst, "mode");
				var txt = ReadTextNodes($$("data", p[z])[0]);

				var i = $A(dst, "zone");
				if (($A(dst, "create") == "true" || _o_acl) &&
					i.substr(0, 2) == "wa" && !$(i)) {
					
					CreateLayer(i);
				}

				f = f || i;
				g = g || $A(dst, "go");		// For compatibility with older version
				i = $(i || dst.firstChild.nodeValue);	// For compatibility with older version

				/* if we target the active layer, remove buttons */
				if (!k && a && a.id == i.id) {
					HideHeader();
					DelLayerButtons(i);
					k = i;
				}
				
				/* Rset scroll if we modify a previous layer */
				ResetScroll(i.id);

				/* update content */
				SetContent(i, txt, mod);
			}

			/* Custom title for the given layer */
			t = $$("title", o);
			for (var n = 0; n < t.length; n++) {
				var s = $($A(t[n], "set"));
				s.title = ReadTextNodes(t[n]);
				if (a == s) { SetTitle(); }
			}

			/* active layer is targeted? show new header */
			if (k) {
				AddLayerButtons(k);
				ShowHeader();
			}

			/* script to execute */
			var e = $$("script", o)[0];
			if (e) { NewScript(ReadTextNodes(e)); }

			/* initialize stuff if required */
			InitForms(a);
			SetBackButton();

			if (g == a) { g = null; }	// let ImagesShow work properly
			if (!g) { ImagesShow(); }

			return [f, g ? Go(g) : null];
		}

		throw "Invalid asynchronous response received.";
	}

	function SetContent(o, c, m) {
		c = ImagesParse(c);
		// Store content in a temp <DIV> to prepare script execution and prepare images
		c = NewItem("div", c);
		// Clone the <DIV> so that Safari properly recognize <SCRIPT> tags
		c = c.cloneNode(true);
		// replace images with blank content to speed up loading!
		ImagesInit(c);

		// Append content to webapp
		if (m == "replace" || m == "append") {
			if (m != "append") {
				o.innerHTML = "";
			}
			while (c.hasChildNodes()) {
				o.appendChild(c.firstChild);
			}
		} else {
			var p = o.parentNode;
			var w = (m == "before") ? o : o.nextSibling;

			if (m == "self") {
				DelItem(p, o);
			}
			while (c.hasChildNodes()) {
				p.insertBefore(c.firstChild, w);
			}
		}
	}

	function __callback(o, cb, lr) {
		if (o.readyState != 4) {
			return;
		}
		var er, ld, ob;

		if (ob = _ajax.filter(function(a) { return o == a[0] })[0]) {
			CallListeners("endasync", ob.pop(), ob[0]);
			Remove(_ajax, ob);
		}

		er = (o.status != 200 && o.status != 0); // 0 for file based requests
		try { if (cb) { ld = cb(o, lr, DefaultCallback()); } } 
		catch (ex) { er = ex; console.error(er); }

		if (lr) {
			$pc.Loader(lr, 0);
			if (er) { DelClass(lr, "__sel", "__tap"); }
		}
	}

/* Render */
	function InitHeader() {
		var hd = _header[$h.HEAD];
		if (hd) {
			var dv = NewItem("div");
			dv.style.opacity = 1;
			while (hd.hasChildNodes()) {
				dv.appendChild(hd.firstChild);
			}
			hd.appendChild(dv);
			_head = dv;

			Display(dv, 1);
			Display(_header[$h.TITLE], 1);
		}
	}

	function InitTab() {
		var o = $$("ul");
		for(var i = 0; i < o.length; i++) {
			var p = o[i].parentNode;
			if (p && HasClass(p, "iTab")) {
				Display(o[i].id + "-loader", 0);
				ShowTab(o[i], $$("li", o[i])[0]);
			}
		}
	}

	function UpdateRadio(r, p) {
		for (var j = 0; j < r.length; j++) {
			with (r[j])	{
				if  (type == "radio" &&
					(checked || getAttribute("checked"))) {	// Safari bug, have to use getAttribute and set checked state from async content

					checked = true;
					p = $$("span", p || GetParent(r[j], "li"))[0];
					p.innerHTML = GetText(parentNode);
					break;
				}
			}
		}
	}

	function InitRadio(p) {
		var o = $$("li", p);	// TODO: use getElementsByClassName

		// search each <li> for unprocessed iRadio
		for (var i = 0; i < o.length; i++) {
			if (HasClass(o[i], "iRadio") && !HasClass(o[i], "__done")) {
				var lnk = NewItem("a");
				var sel = NewItem("span");
				var inp = $$("input", o[i]);
				lnk.appendChild(sel);
				while (o[i].hasChildNodes()) {
					lnk.appendChild(o[i].firstChild);
				}
				o[i].appendChild(lnk);

				lnk.href = "#";
				AddClass(o[i], "__done");
				UpdateRadio(inp, o[i]);
			}
		}

		// create the layer to show radio selection
		var s = "wa__radio";
		if (!$(s)) {
			CreateLayer(s);
		}
	}

	function ClickRadio(a, u) {
		var p = _radio;
		var x = $$("input", p);
		var y = $$("a", u);

		for (var i = 0; i < y.length; i++) {
			if (y[i] == a) {
				if (x[i].disabled) {
					return false;
				}
				var c = x[i].onclick;
				if (c && c() === false) {
					return false;
				}
				x[i].checked = true;
				UpdateRadio([x[i]]);
				//$$("span", p)[0].innerHTML = GetText(x[i].parentNode);
				if ($A(p, "value") == "autoback") {
					setTimeout($pc.Back, 0);
				}
				break;
			}
		}
	}

	function ShowRadio(p) {
		var o = $$("input", p);
		var dv = NewItem("div");
		var ul = NewItem("ul");
		ul.className = "iCheck";
		_radio = p;

		for (var i = 0; i < o.length; i++) {
			if (o[i].type == "radio") {
				var li = NewItem("li");
				var a = NewItem("a", o[i].nextSibling.nodeValue); // TODO: is that correct???

				a.href = "#";
				li.appendChild(a);
				ul.appendChild(li);
				if (o[i].checked)	{ AddClass(li, "__act"); }
				if (o[i].disabled)	{ AddClass(li, "__dis"); }
			}
		}

		dv.className = "iMenu";
		dv.appendChild(ul);

		o = $("wa__radio");
		if (o.firstChild) {
			DelItem(o, o.firstChild);
		}
		o.title = GetText(p.firstChild);
		o.appendChild(dv);
	}

	function InitObj(i) {
		var o = NewItem("div");
		o.id = i;
		_webapp.appendChild(o);
		return o;
	}

	function InitCheck(p) {
		var o = $$("input", p);

		for (var i = 0; i < o.length; i++) {
			if (o[i].type == "checkbox" && HasClass(o[i], "iToggle") && !HasClass(o[i], "__done")) {
				o[i].id		= o[i].id || "__" + Math.random();
				o[i].title	= o[i].title || "ON|OFF";

				var txt = o[i].title.split("|");

				var b1 = NewItem("b", "&nbsp;");
				var b2 = NewItem("b");
				var i1 = NewItem("i", txt[1]);

				b1.className = "iToggle";
				b1.title = o[i].id;
				b1.appendChild(b2);
				b1.appendChild(i1);
				o[i].parentNode.insertBefore(b1, o[i]);
				b1.onclick = function() { FlipCheck(this) };
				FlipCheck(b1, 1);
				AddClass(o[i], "__done");
			}
		}
	}

	function IsViewable(o) {
		var x11, x12, y11, y12;
		var x21, x22, y21, y22;

		var p = XY(o);

		x11 = p.x;
		y11 = p.y;
		x12 = x11 + o.offsetWidth - 1;
		y12 = y11 + o.offsetHeight - 1;

		p = WIN();
		x21 = p.x;
		y21 = p.y;
		x22 = x21 + p.w - 1;
		y22 = y21 + p.h - 1;

		return !(x11 > x22 || x12 < x21 || y11 > y22 || y12 < y21);
	}

/* Form custom elements */
	function InitForms(l) {
		l = $(l) || GetActive();
		InitCheck(l);
		InitRadio(l);
	}

/* Progressive images loading */
	function ImagesParse(c) {
		if (_pil) { c = c.replace(/(.*?<img.*?(\s|\t)*?)src(=.+?>.*?)/g, "$1$2load$3"); }
		return c;
	}

	function ImagesInit(c) {
		if (_pil) {
			var p, tmp = $$("img", c);
			for (var i = 0; i < tmp.length; i++) {
				// Ignore elements with button parent
				if ( (p = GetParent(tmp[i], "a")) &&
						(	HasToken(p.rel, "action") ||
							HasToken(p.rel, "back")) ) {
					ImagesLoad(tmp[i], 1);
				} else {
					tmp[i].src = _blank;
				}
			}
		}
	}

	function ImagesLoad(i, c) {
		var o = $A(i, "load");
		if (o && c) {
			i.removeAttribute("load");
			i.src = o;
		}
	}

	function ImagesCheck() {
		if (_scrAmount - WIN().y == 0) {
			_scrID = clearInterval(_scrID);	// no return = disable _scrID
			ImagesShow();
		}
	}

	function ImagesShow() {
		if (_pil) {
			var img = $$("img", GetActive());
			for (var i = 0; i < img.length; i++) {
				ImagesLoad(img[i], IsViewable(img[i]));
			}
		}
	}

	function ImagesListener() {
		_scrolled = 1;
		if (_pil && !_sliding) {
			if (!_scrolling) {
				_scrolling = true;
				setTimeout(function() {
					_scrAmount = WIN().y;
					_scrolling = false;
				}, 500);
			}
			if (!_scrID) { _scrID = setInterval(ImagesCheck, 1000); }
		}
	}

/* PreLoad */

	function DocumentTracker(s) {
		addEventListener(s, Resizer, false);		
	}

	addEventListener("load", Init, true);
	addEventListener("click", ListenClick, true);

/* Static */
	return $pc;
})();

var WA = WebApp;