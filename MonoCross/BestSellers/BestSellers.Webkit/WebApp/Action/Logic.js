var WebApp=(function(){var A_=setTimeout;var B_=setInterval;var _nn,_oo,_pp,_pper;var _rr,_ss,_tt,_uu,_vv;var _ww,_xx;var _yy,_zz,_00;var _11,_22;var _33=-1;var _44=-1;var _55=[];var _66=[];var _77=[];var _88=[];var _99=history.length;var _AAA=0;var _BBB=0;var _CCC="";var _DDD="";var _EEE=0;var _FFF=0;var _GGG=1;var _HHH=null;var _III=1;var _JJJ="";var _KKK=0;var _LLL=B_(_f,250);var _MMM=null;var _NNN=window;var _OOO="data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==";var _wkt;var _PPP=!!document.getElementsByClassName&&UA("WebKit");var _QQQ=!!navigator.standalone;var _RRR=_N(_NNN.ontouchstart)&&!UA("Android");var _SSS=_RRR?_3:_4;var _TTT={}
_TTT.load=[];_TTT.beginslide=[];_TTT.endslide=[];_TTT.beginasync=[];_TTT.willasync=[];_TTT.endasync=[];_TTT.orientationchange=[];_TTT.tabchange=[];var _UUU=false;var d=document;var $h={get HEAD(){return 0},get BACK(){return 1},get HOME(){return 2},get LEFT(){return 3},get RIGHT(){return 4},get TITLE(){return 5}}
var $d={get L2R(){return+1},get R2L(){return-1}}
d.wa={get auto_HH(){return _UUU},set auto_HH(v){_UUU=(v=="true"||v=="yes"||v===true)},get header(){return $h},get direction(){return $d}}
d.webapp=d.wa;var $pc={get Version(){return 'v0.5.2'},Proxy:function(url){_JJJ=url},Progressive:function(enable){_KKK=enable},Opener:function(func){_11=func?func:function(u){location=u}},Refresh:function(id){if(id!==false){var o=$(id);if(!o){_ff()}else if(o.type=="radio"){_YY([o])}else if(o.type=="checkbox"){_DD(o.previousSibling,1)}}
_AA();_1();_q()},HideBar:function(){if(_III&&_p()){_III=0;_A(1);A_(_A,0)}return false},Header:function(show,what,keep){_CC();if(keep!=-1){_D(!show,keep)}
_h(_oo,0);_oo=$(what);_h(_oo,show);_pper[$h.HEAD].style.zIndex=!show?2:"";return false},Tab:function(id,active){var o=$(id);_u(o,$$("li",o)[active])},AddEventListener:function(evt,handler){if(_N(_TTT[evt])){with(_TTT[evt]){if(indexOf(handler)==-1){push(handler)}}
}},RemoveEventListener:function(evt,handler){if(_N(_TTT[evt])){with(_TTT[evt]){splice(lastIndexOf(handler),1)}}
},Back:function(){if(_BBB){return(_BBB=0)}
_22=null;if(history.length-_99==_44){history.back()}else{_11(_55[_44-1][1])}return false},Home:function(){if(history.length-_99==_44){history.go(-_44)}else{_11("#")}return(_BBB=0)},Form:function(frm,focus){var s,a,b,c,o,k,f,t,i;a=$(frm);b=$(_55[_44][0]);s=(a.style.display!="block");f=_T(a);with(_pper[$h.HEAD]){t=offsetTop+offsetHeight}if(s){a.style.top=t+"px"}if(f){k=f.onsubmit;if(!s){f.onsubmit=f.onsubmit(0,1)}else{f.onsubmit=function(e,b){if(b){return k}if(e){_K(e)}if(!(k&&k(e)===false)){$pc.Submit(this,null,e)}}
}}
_h(a,s);_k(s,t+a.offsetHeight);o=$$("legend",a)[0];_AA(s&&o?o.innerHTML:null);_HHH=(s)?a:null;if(s){c=a;a=b;b=c}
_F(a);_E(b,s);if(s){$pc.Header();if(focus&&(i=$$("input",f)[0])){i.focus()}}else{_D(!s)}return false},Submit:function(frm){var f=_T(frm);if(f){var a=arguments[1];var _=function(i,f){var q="";for(var n=0;n<i.length;n++){i[n].blur();if(i[n].name&&!i[n].disabled&&(f?f(i[n]):1)){q+="&"+i[n].name+"="+encodeURIComponent(i[n].value)}}return q}
var q=_($$("input",f),function(i){with(i){return((_O(type,["text","password","hidden","search"])||(_O(type,["radio","checkbox"])&&checked)))}}
);q+=_($$("select",f));q+=_($$("textarea",f));q+="&"+(a&&a.id?a.id:"__submit")+"=1";q=q.substr(1);var u=($A(f,"action")||self.location.href);if($A(f,"method").toLowerCase()!="post"){u=_NN(u,q);q=null}
_OO(u,null,q);if(_HHH){$pc.Form(_HHH)}}return false},Postable:function(keys,values){var q="";for(var i=1;i<values.length&&i<=keys.length;i++){q+="&"+keys[i-1]+"="+encodeURIComponent(values[i])}return q.replace(/&=/g,"&").substr(1)},Request:function(url,prms,cb,async,loader){if(_44===cb){return}
var r,a=[url,prms];if(!_s("beginasync",a)){if(loader){A_(_a,100,loader,"__sel")}}else{url=a[0];prms=a[1];cb=cb==-1?_PP():cb;var o=new XMLHttpRequest();var c=function(){_VV(o,cb,loader)}
var m=prms?"POST":"GET";async=!!async;if(loader){$pc.Loader(loader,1)}
_88.push([o,a]);url=_MM(url,"__async","true");if(_44>=0){url=_MM(url,"__source",_55[_44][0])}
url=_KK(url);o.open(m,url,async);if(prms){o.setRequestHeader("Content-Type","application/x-www-form-urlencoded")}
_s("willasync",a,o);o.onreadystatechange=(async)?c:null;o.send(prms);if(!async){c()}}
},Loader:function(obj,show){var o,h,f;if(o=$(obj)){h=_X(o,"__lod");_C(o);if(show){if(h){$pc.Loader(obj,0)}
_Z(o,"__lod");_66.push([o,_H(o)])}else if(h){_a(o,"__lod");f=_66.filter(function(f){return f[0]==o}
)[0];_d(_66,f);if(f=f[1]){f[0]._=0;clearInterval(f[1]);f[0].style.backgroundImage=""}}
}return h},Player:function(src){if(!_p()){_NNN.open(src)}else{if(_PPP){location="#"+Math.random()}
var w=$("__wa_media");var o=_R("iframe");o.id="__wa_media";o.src=src;_rr.appendChild(o);_S(w)}return false},toString:function(){return "[WebApp.Net Framework]"}}
function _A(h){h=h?h:0;_rr.style.minHeight=(_FFF+h)+"px";_NNN.scrollTo(0,h)}
function _B(s,w,dir,step,mn){s+=Math.max((w-s)/step,mn||4);return[s,(w+w*dir)/2-Math.min(s,w)*dir]}
function _C(o){if(_X(o,"iMore")){var a=$$("a",o)[0];if(a&&a.title){var s=$$("span",a)[0]||a;o=s.innerHTML;s.innerHTML=a.title;a.title=o}}
}
function _D(s,k){if(_pp){var h=_pper;k=(s)?[]:k||[];for(var i=1;i<h.length;i++){if(!_O(i,k)){_h(h[i],s)}}
with($h){if(!_O(BACK,k)){_h(h[BACK],s&&!h[LEFT]&&_44)}if(!_O(HOME,k)){_h(h[HOME],s&&!h[RIGHT]&&!_BBB&&_44>1)}}
}}
function _E(lay,ignore){if(_pp){var a=$$("a",lay);var p=$h.RIGHT;for(var i=0;i<a.length&&p>=$h.LEFT;i++){if(_pper[p]&&!ignore){i--;p--;continue}if(_W(a[i].rel,"action")||_W(a[i].rel,"back")){_Z(a[i],p==$h.RIGHT?"iRightButton":"iLeftButton");_h(a[i],1);_pper[p--]=a[i];_pp.appendChild(a[i--])}}
}}
function _F(lay){if(_pp){with($h){for(var i=LEFT;i<=RIGHT;i++){var a=_pper[i];if(a&&(_W(a.rel,"action")||_W(a.rel,"back"))){_h(a,0);_a(a,i==RIGHT?"iRightButton":"iLeftButton");lay.insertBefore(a,lay.firstChild)}}
_pper[RIGHT]=$("waRightButton");_pper[LEFT]=$("waLeftButton")}}
}
function _G(o){var u;if(u=getComputedStyle(o).backgroundImage){o._=1;return/(.+?(\d+)x(\d+)x)(\d+)(.*)/.exec(u)}}
function _H(o){var d,c,i;if(!(d=_G(o))){c=$$("*",o);for(i=0;i<c.length;i++){o=c[i];if(d=_G(o)){break}}
}return(d)?[o,B_(_I,d[2],[o,d[4],d[3],(d[1]+"*"+d[5]),new Image()])]:d}
function _I(a){if(!a[5]){a[1]=parseInt(a[1])% parseInt(a[2])+1;var b=a[3].replace("*",a[1]);a[4].onload=function(){if(a[0]._)a[0].style.backgroundImage=b;a[5]=0}
a[5]=a[4].src=b.substr(4,b.length-5)}}
function _J(s){return s.replace(/<.+?>/g,"").replace(/^\s+|\s+$/g,"").replace(/\s{2,}/," ")}
function _K(e){e.preventDefault();e.stopPropagation()}
function _L(o){return _W(o.rev,"async")||_W(o.rev,"async:np")}
function _M(o){return _W(o.rev,"media")}
function _N(o){return(typeof o!="undefined")}
function _O(o,a){return a.indexOf(o)!=-1}
function $(i){return typeof i=="string"?document.getElementById(i):i}
function $$(t,o){return(o||document).getElementsByTagName(t)}
function $A(o,a){return o.getAttribute(a)||""}
function XY(e){var x=0;var y=0;while(e){x+=e.offsetLeft;y+=e.offsetTop;e=e.offsetParent}return{x:x,y:y}}
function _P(){with(_NNN)return{x:pageXOffset,y:pageYOffset,w:innerWidth,h:innerHeight}}
function _Q(c){var s,h=$$("head")[0];s=_R("script");s.type="text/javascript";s.textContent=c;h.appendChild(s)}
function _R(t,c){var o=document.createElement(t);if(c){o.innerHTML=c}return o}
function _S(p,c){if(p){if(!c){c=p;p=c.parentNode}
p.removeChild(c)}}
function _T(o){o=$(o);if(o&&_V(o)!="form"){o=_b(o,"form")}return o}
function _U(o){return _V(o)=="a"?o:_b(o,"a")}
function _V(o){return o.localName.toLowerCase()}
function _W(o,t){return o&&_O(t,o.toLowerCase().split(" "))}
function _X(o,c){return o&&_O(c,_Y(o))}
function _Y(o){return o.className.split(" ")}
function _Z(o,c){var h=_X(o,c);if(!h){o.className+=" "+c}return h}
function _a(o){var c=_Y(o);var a=arguments;for(var i=1;i<a.length;i++){_d(c,a[i])}
o.className=c.join(" ")}
function _b(o,t){while((o=o.parentNode)&&(o.nodeType!=1||_V(o)!=t)){}return o}
function _c(o,c){while((o=o.parentNode)&&(o.nodeType!=1||!_X(o,c))){}return o}
function _d(a,e){var p=a.indexOf(e);if(p!=-1){a.splice(p,1)}}
function _e(o){o=o.childNodes;for(var i=0;i<o.length;i++){if(o[i].nodeType==3){return o[i].nodeValue.replace(/^\s+|\s+$/g,"")}}return null}
function _f(){if(!_rr){_rr=$("WebApp")}if(!_ss){_ss=$("iGroup")}
var i=$("iLoader");if(i&&!_X(i,"__lod")){$pc.Loader(i,1)}}
function _g(){_pper=[$("iHeader"),$("waBackButton"),$("waHomeButton"),$("waLeftButton"),$("waRightButton"),$("waHeadTitle")];_uu=document.body;_tt=(_uu.dir=="rtl")?-1:+1;_wkt=_N(_uu.style.webkitTransform)}
function _h(o,s){if(o=$(o)){o.style.display=s?"block":"none"}}
function _i(o,s){if(o=$(o)){o.style[_tt==1?"left":"right"]=s?0:"";o.style.display=s?"block":""}}
function _j(o){if(o=o||_JJ()){var z=$$("div",o);z=z[z.length-1];if(z&&(_X(z,"iList")||_X(z,"iFull"))){z.style.minHeight=parseInt(_rr.style.minHeight)-XY(z).y+"px"}}
}
function _k(s,p){var o=$("__wa_shadow");o.style.top=p+"px";_rr.style.position=s?"relative":"";_h(o,s)}
function _l(o,l){if(o){_55.splice(++_44,_55.length);_55.push([o,!l?location.hash:("#_"+_nn.substr(2)),_GGG])}}
function _m(o){var s=$$("script",o);while(s.length){_S(s[0])}return o}
function _n(){var s,i,c;while(_66.length){$pc.Loader(_66[0][0],0)}
s=$$("li");for(i=0;i<s.length;i++){_a(s[i],"__sel","__tap")}}
function _o(s,np){var ed=s.indexOf("#_");if(ed==-1){return null}
var rs="";var bs=_LL(s);if(!np){for(var i=0;i<bs[1].length;i++){rs+="/"+bs[1][i].split("=").pop()}}return bs[2]+rs}
function _p(){return(UA("iPhone")||UA("iPod")||UA("Aspen"))}
function UA(s){return _O(s,navigator.userAgent)}
function _q(){if(_AAA){return}
var m,h,o,w=(_P().w>=_xx)?_xx:_ww;if(w!=_EEE){_EEE=w;_rr.className=(w==_ww)?"portrait":"landscape";_s("orientationchange")}if(o=_JJ()){h=XY(o).y+o.offsetHeight}
m=_EEE==_ww?416:268;w=_P().h;h=h<w?w:h;h=h<m?m:h;_FFF=h;_rr.style.minHeight=h+"px";_j()}
function _r(){if(_AAA||_BBB==location.href){return}
_BBB=0;var act=_JJ();if(act){act=act.id}else if(location.hash.length>0){return}else{act=_55[0][0]}
var cur=_55[_44][0];if(act!=cur){var i,pos=-1;for(i in _55){if(_55[i][0]==act){pos=parseInt(i);break}}if(pos!=-1&&pos<_44){_0(cur,act,$d.L2R)}else{_z(act)}}
}
function _s(evt,ctx,obj){var l=_TTT[evt].length;if(l==0){return true}
var e={type:evt,target:obj||null,context:ctx||_FF(_55[_44][1]),windowWidth:_EEE,windowHeight:_FFF}
var k=true;for(var i=0;i<l;i++){k=k&&(_TTT[evt][i](e)==false?false:true)}return k}
function _t(){clearInterval(_LLL);_f();_g();_ff();_XX();_WW();_cc("__wa_shadow");var i=$("iLoader");$pc.Loader(i,0);_S(i);$pc.Opener(_11);_ww=screen.width;_xx=screen.height;if(_ww>_xx){var l=_xx;_xx=_ww;_ww=l}
_nn=_GG()[0].id;_l(_nn,1);var a=(_JJ()||"").id;if(a!=_nn){_l(a)}if(!a){a=_nn;_11("#")}
_i(a,1);_E($(a));with($h){var h=_pper;_h(h[BACK],(!h[LEFT]&&_44));_h(h[HOME],(!h[RIGHT]&&_44>1&&a!=_nn));if(h[BACK]){_DDD=h[BACK].innerHTML}if(h[TITLE]){_CCC=h[TITLE].innerHTML;_AA()}}
B_(_r,250);_s("load");_rr.addEventListener("touchstart",new Function(),false);(_RRR?_ss:document).addEventListener(_RRR?"touchmove":"scroll",_ll,false);_q();_kk();_mm("DOMSubtreeModified");_mm("resize");$pc.HideBar()}
function _u(ul,li,h,ev){if(!(_X(li,"__dis")||_W($$("a",li)[0].rel,"action"))){var c,s,al=$$("li",ul);for(var i=0;i<al.length;i++){c=(al[i]==li);if(c){s=i}
_h(ul.id+i,(!h&&c));_a(al[i],"__act")}
_Z(li,"__act");if(ev){_s("tabchange",[s],ul)}}
}
function _v(o){if(o)o.style.webkitTransform=""}
function _w(e){if(_AAA){return _K(e)}
var o=e.target;var n=_V(o);if(n=="label"){var f=$($A(o,"for"));if(_X(f,"iToggle")){A_(_DD,1,f.previousSibling,1)}return}
var li=_b(o,"li");if(li&&_X(li,"iRadio")){_Z(li,"__sel");_bb(li);_BBB=location.href;_z("wa__22");return _K(e)}
var a=_U(o);if(a&&li&&_X(li,"__dis")){return _K(e)}if(a&&a.onclick){var old=a.onclick;a.onclick=null;var val=old.call(a,e);A_(function(){a.onclick=old},0);if(val===false){if(li){_Z(li,_X(a,"iSide")?"__tap":"__sel");_x(li)}return _K(e)}}
var ul=_b(o,"ul");var pr=!ul?null:ul.parentNode;var ax=a&&_L(a);if(a&&ul&&_X(pr,"iTab")){var h,t;t=_W(a.rel,"action");h=$(ul.id+"-loader");_h(h,0);if(!t&&ax){_h(h,1);$pc.Loader(h,1);_OO(a,function(o){_h(h,0);$pc.Loader(h,0);_h(_TT(o)[0],1);_u(ul,li,0,1)}
)}else{h=t}
_u(ul,li,!!h,!ax);if(!t){return _K(e)}}if(a&&_O(a.id,["waBackButton","waHomeButton"])){if(a.id=="waBackButton"){$pc.Back()}else{$pc.Home()}return _K(e)}if(ul&&_X(ul,"iCheck")){if(_aa(a,ul)!==false){var al=$$("li",ul);for(var i=0;i<al.length;i++){_a(al[i],"__act","__sel")}
_Z(li,"__act __sel");A_(_a,1000,li,"__sel")}return _K(e)}if(ul&&!_X(li,"iMore")&&((_X(ul,"iMenu")||_X(pr,"iMenu"))||(_X(ul,"iList")||_X(pr,"iList")))){if(a&&!_X(a,"iButton")){var c=_Z(li,_X(a,"iSide")?"__tap":"__sel");if(ax){if(!c){_OO(a)}return _K(e)}}
}
var dv=_c(o,"iMore");if(dv){if(!_X(dv,"__lod")){$pc.Loader(dv,1);if(ax){_OO(a)}}return _K(e)}if(a&&_HHH){if(_W(a.rel,"back")){$pc.Form(_HHH,a);return _K(e)}if(_W(a.rel,"action")){var f=_T(_HHH);if(f){f.onsubmit(e);return}}
}if(a&&_M(a)){_x(li);$pc.Player(a.href,a);return _K(e)}if(ax){_OO(a);_K(e)}else if(a&&!a.target){if(_y(a.href,"http:","https:","file:")){_RR(a.href);_K(e)}
_x(li)}}
function _x(li){if(li){A_(_a,1000,li,"__sel","__tap")}}
function _y(s1){var r,i,a=arguments;for(i=1;i<a.length;i++){if(s1.toLowerCase().indexOf(a[i])==0){return 1}}
}
function _z(to){var h=_55[_44][0];if(h!=to){_0(h,to)}}
function _0(src,dst,dir){if(_AAA){return}
_AAA=1;_EE();if(dst==_55[0][0]){_99=history.length}
dir=dir||$d.R2L;src=$(src);dst=$(dst);var h;if(_wkt&&_pp){h=_m(_pp.cloneNode(true))}
_33=_44;if(dir==$d.R2L){_l(dst.id)}else{while(_44&&_55[--_44][0]!=dst.id){}}
_BB();_F(src);_E(dst);_CC();if(h){_pper[$h.HEAD].appendChild(h)}
_1((dir!=$d.R2L)?"":(_BBB?"":_J(src.title))||_DDD);_AA(_BBB?dst.title:null);_8(src,dst,dir)}
function _1(txt){if(_pper[$h.BACK]){if(!txt&&_44){txt=_J($(_55[_44-1][0]).title)||_DDD}if(txt){_pper[$h.BACK].innerHTML=txt}}
}
function _2(m){var s=_FF(_55[_33][1]);var d=_FF(_55[_44][1]);var r=(m<0&&!!_BBB)?["wa__22"]:d;return[s,d,m,r]}
function _3(t){return "translate3d("+t+",0,0)"}
function _4(t){return "translateX("+t+")"}
function _5(o,t,i){if(o){if(t){t=_SSS(t)}
o.style.webkitTransitionProperty=(i)?"none":"";o.style.webkitTransform=t}}
function _6(o){return o?getComputedStyle(o,null).webkitTransitionDuration:"0s"}
function _7(){var r,t,i,j,a=arguments;r=0;for(i=0;i<a.length;i++){t=_6(a[i]).split(',');for(j=0;j<t.length;j++){r=Math.max(r,parseFloat(t[j])*1000)}}return r}
function _8(src,dst,dir){_s("beginslide",_2(dir));_ff(dst);_i(src,1);_i(dst,1);if(!_wkt){_9(src,dst,dir);return}
var b=_ss;var w=_rr;var g=dir*_tt;b.style.height=(_FFF-b.offsetTop)+"px";_Z(w,"__ani");_5(src,"0",1);_5(dst,(g*-100)+"%",1);var h,hcs,hos,tim=_7(src,dst,_pp,_pper[$h.TITLE]);if(_pp){h=_pper[$h.HEAD].lastChild;hcs=h.style;hos=_pp.style;hcs.opacity=1;hos.opacity=0;_5(h,"0",1);_5(_pp,(g*-20)+"%",1);_5(_pper[$h.TITLE],(g==$d.R2L?60:-20)+"%",1)}
A_(function(){_j(dst);_5(dst,"0");_5(src,(g*100)+"%");if(h){hcs.opacity=0;hos.opacity=1;_5(h,(g*30)+"%");_5(_pp,"0");_5(_pper[$h.TITLE],"0")}
A_(function(){if(h){_S(_pper[$h.HEAD],h)}
_a(w,"__ani");b.style.height="";_9(src,dst,dir)},tim)},0)}
function _9(src,dst,dir){_n();_i(src,0);if(_wkt){_v(dst);_v(src);_v(_pp);_v(_pper[$h.TITLE])}
_s("endslide",_2(dir));_AAA=0;_33=-1;_q();A_(_EE,0,dir==$d.L2R?_55[_44+1][2]:null);A_(_kk,0)}
function _AA(title){var o;if(o=_pper[$h.TITLE]){o.innerHTML=title||_II(_JJ())||_CCC}}
function _BB(){if(_HHH){$pc.Form(_HHH)}
_h(_oo,0)}
function _CC(){_D(1)}
function _DD(o,dontChange){var c=o,i=$(c.title);var txt=i.title.split("|");if(!dontChange){i.click()}
(i.disabled?_Z:_a)(c,"__dis");o=c.firstChild.nextSibling;with(c.lastChild){innerHTML=txt[i.checked?0:1];if(i.checked){o.style.left="";o.style.right="-1px";_Z(c,"__sel");style.left=0;style.right=""}else{o.style.left="-1px";o.style.right="";_a(c,"__sel");style.left="";style.right=0}}
}
function _EE(to){var h=to?to:Math.min(50,_P().y);var s=to?Math.max(1,to-50):1;var d=to?-1:+1;while(s<=h){var z=_B(s,h,d,6,2);s=z[0];_NNN.scrollTo(0,z[1])}if(!to){$pc.HideBar()}}
function _FF(loc){if(loc){var p=loc.indexOf("#_");if(p!=-1){loc=loc.substring(p+2).split("/");var id="wa"+loc[0];for(var i in loc){loc[i]=decodeURIComponent(loc[i])}
loc[0]=id;if(_UUU&&!$(id)){_HH(id)}return $(id)?loc:[]}}return[]}
function _GG(){var lay=[];var src=_ss.childNodes;for(var i in src){if(src[i].nodeType==1&&_X(src[i],"iLayer")){lay.push(src[i])}}return lay}
function _HH(i){var n=_R("div");n.id=i;n.className="iLayer";_ss.appendChild(n);return n}
function _II(o){return(!_44&&_CCC)?_CCC:o.title}
function _JJ(){var h=location.hash;return $(!h?_nn:_FF(h)[0])}
function _KK(url){var d=url.match(/[a-z]+:\/\/(.+:.*@)?([a-z0-9-\.]+)((:\d+)?\/.*)?/i);return(!_JJJ||!d||d[2]==location.hostname)?url:_MM(_JJJ,"__url",url)}
function _LL(u){var s,q,d;s=u.replace(/&amp;/g,"&");d=s.indexOf("#");d=s.substr(d!=-1?d:s.length);s=s.substr(0,s.length-d.length);q=s.indexOf("?");q=s.substr(q!=-1?q:s.length);s=s.substr(0,s.length-q.length);q=!q?[]:q.substr(1).split("&");return[s,q,d]}
function _MM(u,k,v){u=_LL(u);var q=u[1].filter(function(o){return o&&o.indexOf(k+"=")!=0}
);q.push(k+"="+encodeURIComponent(v));return u[0]+"?"+q.join("&")+u[2]}
function _NN(u,q){u=_LL(u);u[1].push(q);return u[0]+"?"+u[1].join("&")+u[2]}
function _OO(item,cb,q){var h,o,u,i;i=(typeof item=="object");u=(i?item.href:item);o=_b(item,"li");if(!cb){cb=_PP(u,_W(item.rev,"async:np"))}
$pc.Request(u,q,cb,true,o,(i?item:null))}
function _PP(i,np){return function(o){var u=i?_o(i,np):null;var g=_TT(o);if(g&&(g[1]||u)){_RR(g[1]||u)}else{_n()}return null}}
function _QQ(o){var nds=o.childNodes;var txt="";for(var y=0;y<nds.length;y++){txt+=nds[y].nodeValue}return txt}
function _RR(l){_GGG=_P().y;_EE();_11(l)}
function Go(g){return "#_"+g.substr(2)}
function _SS(i){if(i.substr(0,2)=="wa"){var p=_44;if(p&&i==_55[0][0]){_55[1][2]=0}
while(p&&_55[--p][0]!=i){}if(p){_55[p+1][2]=0}}
}
function _TT(o){if(o.responseXML){o=o.responseXML.documentElement;var s,t,k,a=_JJ();var g=$$("go",o);g=(g.length!=1)?null:$A(g[0],"to");var f,p=$$("part",o);if(p.length==0){p=[o]}
for(var z=0;z<p.length;z++){var dst=$$("destination",p[z])[0];if(!dst){break}
var mod=$A(dst,"mode");var txt=_QQ($$("data",p[z])[0]);var i=$A(dst,"zone");if(($A(dst,"create")=="true"||_UUU)&&i.substr(0,2)=="wa"&&!$(i)){_HH(i)}
f=f||i;g=g||$A(dst,"go");i=$(i||dst.firstChild.nodeValue);if(!k&&a&&a.id==i.id){_BB();_F(i);k=i}
_SS(i.id);_UU(i,txt,mod)}
t=$$("title",o);for(var n=0;n<t.length;n++){var s=$($A(t[n],"set"));s.title=_QQ(t[n]);if(a==s){_AA()}}if(k){_E(k);_CC()}
var e=$$("script",o)[0];if(e){_Q(_QQ(e))}
_ff(a);_1();if(g==a){g=null}if(!g){_kk()}return[f,g?Go(g):null]}
throw "Invalid asynchronous response received."}
function _UU(o,c,m){c=_gg(c);c=_R("div",c);c=c.cloneNode(true);_hh(c);if(m=="replace"||m=="append"){if(m!="append"){o.innerHTML=""}
while(c.hasChildNodes()){o.appendChild(c.firstChild)}}else{var p=o.parentNode;var w=(m=="before")?o:o.nextSibling;if(m=="self"){_S(p,o)}
while(c.hasChildNodes()){p.insertBefore(c.firstChild,w)}}
}
function _VV(o,cb,lr){if(o.readyState!=4){return}
var er,ld,ob;if(ob=_88.filter(function(a){return o==a[0]}
)[0]){_s("endasync",ob.pop(),ob[0]);_d(_88,ob)}
er=(o.status!=200&&o.status!=0);try{if(cb){ld=cb(o,lr,_PP())}}
catch(ex){er=ex;console.error(er)}if(lr){$pc.Loader(lr,0);if(er){_a(lr,"__sel","__tap")}}
}
function _WW(){var hd=_pper[$h.HEAD];if(hd){var dv=_R("div");dv.style.opacity=1;while(hd.hasChildNodes()){dv.appendChild(hd.firstChild)}
hd.appendChild(dv);_pp=dv;_h(dv,1);_h(_pper[$h.TITLE],1)}}
function _XX(){var o=$$("ul");for(var i=0;i<o.length;i++){var p=o[i].parentNode;if(p&&_X(p,"iTab")){_h(o[i].id+"-loader",0);_u(o[i],$$("li",o[i])[0])}}
}
function _YY(r,p){for(var j=0;j<r.length;j++){with(r[j]){if(type=="radio"&&(checked||getAttribute("checked"))){checked=true;p=$$("span",p||_b(r[j],"li"))[0];p.innerHTML=_e(parentNode);break}}
}}
function _ZZ(p){var o=$$("li",p);for(var i=0;i<o.length;i++){if(_X(o[i],"iRadio")&&!_X(o[i],"__done")){var lnk=_R("a");var sel=_R("span");var inp=$$("input",o[i]);lnk.appendChild(sel);while(o[i].hasChildNodes()){lnk.appendChild(o[i].firstChild)}
o[i].appendChild(lnk);lnk.href="#";_Z(o[i],"__done");_YY(inp,o[i])}}
var s="wa__22";if(!$(s)){_HH(s)}}
function _aa(a,u){var p=_22;var x=$$("input",p);var y=$$("a",u);for(var i=0;i<y.length;i++){if(y[i]==a){if(x[i].disabled){return false}
var c=x[i].onclick;if(c&&c()===false){return false}
x[i].checked=true;_YY([x[i]]);if($A(p,"value")=="autoback"){A_($pc.Back,0)}
break}}
}
function _bb(p){var o=$$("input",p);var dv=_R("div");var ul=_R("ul");ul.className="iCheck";_22=p;for(var i=0;i<o.length;i++){if(o[i].type=="radio"){var li=_R("li");var a=_R("a",o[i].nextSibling.nodeValue);a.href="#";li.appendChild(a);ul.appendChild(li);if(o[i].checked){_Z(li,"__act")}if(o[i].disabled){_Z(li,"__dis")}}
}
dv.className="iMenu";dv.appendChild(ul);o=$("wa__22");if(o.firstChild){_S(o,o.firstChild)}
o.title=_e(p.firstChild);o.appendChild(dv)}
function _cc(i){var o=_R("div");o.id=i;_rr.appendChild(o);return o}
function _dd(p){var o=$$("input",p);for(var i=0;i<o.length;i++){if(o[i].type=="checkbox"&&_X(o[i],"iToggle")&&!_X(o[i],"__done")){o[i].id=o[i].id||"__"+Math.random();o[i].title=o[i].title||"ON|OFF";var txt=o[i].title.split("|");var b1=_R("b","&nbsp;");var b2=_R("b");var i1=_R("i",txt[1]);b1.className="iToggle";b1.title=o[i].id;b1.appendChild(b2);b1.appendChild(i1);o[i].parentNode.insertBefore(b1,o[i]);b1.onclick=function(){_DD(this)}
_DD(b1,1);_Z(o[i],"__done")}}
}
function _ee(o){var x11,x12,y11,y12;var x21,x22,y21,y22;var p=XY(o);x11=p.x;y11=p.y;x12=x11+o.offsetWidth-1;y12=y11+o.offsetHeight-1;p=_P();x21=p.x;y21=p.y;x22=x21+p.w-1;y22=y21+p.h-1;return!(x11>x22||x12<x21||y11>y22||y12<y21)}
function _ff(l){l=$(l)||_JJ();_dd(l);_ZZ(l)}
function _gg(c){if(_KKK){c=c.replace(/(.*?<img.*?(\s|\t)*?)src(=.+?>.*?)/g,"$1$2load$3")}return c}
function _hh(c){if(_KKK){var p,tmp=$$("img",c);for(var i=0;i<tmp.length;i++){if((p=_b(tmp[i],"a"))&&(_W(p.rel,"action")||_W(p.rel,"back"))){_ii(tmp[i],1)}else{tmp[i].src=_OOO}}
}}
function _ii(i,c){var o=$A(i,"load");if(o&&c){i.removeAttribute("load");i.src=o}}
function _jj(){if(_00-_P().y==0){_yy=clearInterval(_yy);_kk()}}
function _kk(){if(_KKK){var img=$$("img",_JJ());for(var i=0;i<img.length;i++){_ii(img[i],_ee(img[i]))}}
}
function _ll(){_III=1;if(_KKK&&!_AAA){if(!_zz){_zz=true;A_(function(){_00=_P().y;_zz=false},500)}if(!_yy){_yy=B_(_jj,1000)}}
}
function _mm(s){addEventListener(s,_q,false)}
addEventListener("load",_t,true);addEventListener("click",_w,true);return $pc}
)();var WA=WebApp;