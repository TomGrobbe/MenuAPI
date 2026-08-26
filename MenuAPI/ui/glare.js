// Port of Rockstar's mp_menu_glare scaleform to canvas 2D. Every number came out of the
// decompiled SWF, and everything is laid out in the scaleform's own 288 by 75 unit space.

const DESIGN_W = 288;
const DESIGN_H = 65;

const DESIGN_BLEED = 10;
const GLINT_CLIP_H = DESIGN_H + DESIGN_BLEED;

const GLOBE_X = 196;
const GLOBE_Y = -9;

const GLOBE_ALPHA = 38 / 256;

const GLOBE_D =
  'M31.4 41.95 L30.75 47.3 30.35 51.45 Q30.0 55.55 29.9 59.9 L68.5 64.0 ' +
  'Q69.75 57.2 70.3 51.55 L71.1 40.35 32.3 36.25 31.4 41.95 M9.45 33.8 ' +
  'Q8.1 39.1 7.5 44.85 6.8 50.9 7.2 57.5 L23.0 59.15 23.4 51.45 23.85 46.6 ' +
  '24.4 41.95 25.4 35.5 9.45 33.8 M32.85 9.25 Q26.5 12.05 21.25 16.6 ' +
  '16.0 21.2 12.45 27.15 L12.5 27.15 26.8 28.7 Q29.15 18.25 32.8 9.45 ' +
  'L32.85 9.25 M92.0 35.65 L92.0 42.6 78.0 41.1 77.2 52.25 Q76.5 58.8 ' +
  '75.35 64.75 L91.2 66.4 92.0 63.3 92.0 74.0 87.85 74.0 88.4 73.1 ' +
  '73.9 71.55 73.3 74.0 66.2 74.0 67.0 70.8 29.9 66.85 30.3 74.0 23.4 74.0 ' +
  '23.0 66.15 8.65 64.6 8.6 64.6 Q10.35 69.55 13.2 74.0 L6.1 74.0 ' +
  'Q-1.35 60.7 0.35 44.65 2.55 23.95 18.75 10.85 L21.2 9.0 40.55 9.0 ' +
  'Q36.4 17.65 33.7 29.4 L71.2 33.4 Q71.0 19.65 68.1 9.5 L68.0 9.2 67.55 9.0 ' +
  'L79.7 9.0 Q85.05 12.9 89.5 18.4 L92.0 21.75 92.0 34.3 Q89.8 28.5 ' +
  '86.05 23.45 81.9 17.85 76.2 13.8 L76.3 14.2 Q77.95 23.35 78.1 34.15 ' +
  'L92.0 35.65 Z';

const GLARE_D =
  'M-53.4 15.75 Q-69.15 -45.9 -97.8 -99.6 L20.2 59.35 -35.9 124.5 ' +
  'Q-40.5 66.5 -53.4 15.75 Z';

const GLARE_GRAD_FROM = [11.66, 45.87];
const GLARE_GRAD_TO = [-62.55, 99.02];

const GLARE_RAMP = [
  1.0, 0.9882, 0.9686, 0.9451, 0.9216, 0.8941, 0.8627, 0.8314, 0.8, 0.7686,
  0.7333, 0.6941, 0.6588, 0.6196, 0.5804, 0.5412, 0.5059, 0.4667, 0.4275,
  0.3882, 0.349, 0.3137, 0.2745, 0.2392, 0.2078, 0.1765, 0.1451, 0.1137,
  0.0863, 0.0627, 0.0392, 0.0196, 0.0078,
];

const GX1 = 230, GR1 = -55, GA1 = 20;
const GX2 = 290, GR2 = -30, GA2 = 20;
const GLARE_Y = -25;

const FPS = 30;
const FRAME_MS = 1000 / FPS;

const GLINT_TOTAL_FRAMES = 270;
const GLINT_LOOP_FRAME = 42;
const GLINT_MIN_DELAY = 60;

const GLINT_FRAMES = [
  {ga:0, m:null, s:[-15.0, 0.25, 0.25]},
  {ga:0.0, m:[-0.44732, -1.6911, 14.89977, -3.94117, -153.55, 225.75], s:[4.75, 0.2901, 0.31943]},
  {ga:0.0312, m:[-0.44732, -1.6911, 14.89977, -3.94117, -148.41667, 226.28333], s:[23.4, 0.3287, 0.38625]},
  {ga:0.0703, m:[-0.44732, -1.6911, 14.89977, -3.94117, -143.28333, 226.81667], s:[40.95, 0.3658, 0.45049]},
  {ga:0.1094, m:[-0.44732, -1.6911, 14.89977, -3.94117, -138.15, 227.35], s:[57.35, 0.40146, 0.51219]},
  {ga:0.1406, m:[-0.44732, -1.6911, 14.89977, -3.94117, -133.01667, 227.88333], s:[72.8, 0.4357, 0.57147]},
  {ga:0.1797, m:[-0.44732, -1.6911, 14.89977, -3.94117, -127.88333, 228.41667], s:[87.25, 0.46857, 0.62837]},
  {ga:0.2188, m:[-0.44732, -1.6911, 14.89977, -3.94117, -122.75, 228.95], s:[100.8, 0.50011, 0.68297]},
  {ga:0.25, m:[-0.44732, -1.6911, 14.89977, -3.94117, -117.61667, 229.48333], s:[113.5, 0.53035, 0.73531]},
  {ga:0.2891, m:[-0.44732, -1.6911, 14.89977, -3.94117, -112.48216, 230.01679], s:[125.45, 0.55934, 0.78552]},
  {ga:0.3281, m:[-0.44732, -1.6911, 14.89977, -3.94117, -107.34883, 230.55012], s:[136.6, 0.58713, 0.8336]},
  {ga:0.3711, m:[-0.44732, -1.6911, 14.89977, -3.94117, -102.21549, 231.08346], s:[147.1, 0.61374, 0.87967]},
  {ga:0.3984, m:[-0.44732, -1.6911, 14.89977, -3.94117, -97.08216, 231.61679], s:[156.9, 0.63922, 0.9238]},
  {ga:0.4414, m:[-0.44732, -1.6911, 14.89977, -3.94117, -91.94883, 232.15012], s:[166.1, 0.66362, 0.96603]},
  {ga:0.4805, m:[-0.44732, -1.6911, 14.89977, -3.94117, -86.81549, 232.68346], s:[174.8, 0.68697, 1.00644]},
  {ga:0.5117, m:[-0.44732, -1.6911, 14.89977, -3.94117, -81.68216, 233.21679], s:[182.95, 0.68407, 1.00815]},
  {ga:0.5508, m:[-0.44732, -1.6911, 14.89977, -3.94117, -76.55, 233.75], s:[190.6, 0.64749, 0.96025]},
  {ga:0.5898, m:[-0.39647, -1.68586, 14.85357, -3.49318, -75.26857, 229.95365], s:[197.85, 0.61253, 0.91447]},
  {ga:0.6211, m:[-0.34564, -1.68062, 14.80738, -3.04535, -73.98762, 226.15868], s:[204.65, 0.57907, 0.87067]},
  {ga:0.6602, m:[-0.2948, -1.67537, 14.76117, -2.59735, -72.70619, 222.36233], s:[211.15, 0.54704, 0.82875]},
  {ga:0.6992, m:[-0.24395, -1.67013, 14.71496, -2.14936, -71.42477, 218.56597], s:[217.3, 0.51642, 0.78865]},
  {ga:0.7383, m:[-0.19312, -1.66488, 14.66877, -1.70153, -70.14381, 214.77101], s:[223.1, 0.48712, 0.75029]},
  {ga:0.7695, m:[-0.14227, -1.65964, 14.62256, -1.25353, -68.86238, 210.97465], s:[228.7, 0.45905, 0.71355]},
  {ga:0.8086, m:[-0.09143, -1.6544, 14.57635, -0.80554, -67.58096, 207.1783], s:[234.0, 0.43214, 0.6783]},
  {ga:0.8516, m:[-0.0406, -1.64915, 14.53016, -0.35771, -66.3, 203.38333], s:[239.15, 0.40634, 0.64453]},
  {ga:0.8789, m:[0.01025, -1.64391, 14.48395, 0.09028, -65.01857, 199.58698], s:[244.05, 0.38159, 0.61214]},
  {ga:0.9219, m:[0.06109, -1.63866, 14.43775, 0.53828, -63.73715, 195.79062], s:[248.85, 0.3578, 0.58098]},
  {ga:0.9609, m:[0.11192, -1.63342, 14.39155, 0.98611, -62.45619, 191.99566], s:[253.45, 0.34384, 0.55101]},
  {ga:1.0, m:[0.16277, -1.62818, 14.34535, 1.4341, -61.17477, 188.1993], s:[257.95, 0.33482, 0.52209]},
  {ga:0.9102, m:[0.21361, -1.62293, 14.29914, 1.8821, -59.89334, 184.40295], s:[262.4, 0.32613, 0.49419]},
  {ga:0.8281, m:[0.26444, -1.61769, 14.25295, 2.32993, -58.61238, 180.60799], s:[266.7, 0.3177, 0.46719]},
  {ga:0.75, m:[0.31529, -1.61244, 14.20674, 2.77792, -57.33096, 176.81163], s:[270.95, 0.30956, 0.44102]},
  {ga:0.6602, m:[0.36614, -1.6072, 14.16053, 3.22591, -56.04953, 173.01528], s:[275.15, 0.30161, 0.41556]},
  {ga:0.5781, m:[0.41696, -1.60196, 14.11434, 3.67374, -54.76857, 169.22031], s:[279.35, 0.29388, 0.39076]},
  {ga:0.5, m:[0.46781, -1.59671, 14.06813, 4.12174, -53.48715, 165.42396], s:[283.45, 0.2863, 0.36647]},
  {ga:0.4102, m:[0.51866, -1.59147, 14.02193, 4.56973, -52.20572, 161.6276], s:[287.6, 0.27888, 0.34265]},
  {ga:0.3281, m:[0.56948, -1.58623, 13.97573, 5.01756, -50.92477, 157.83264], s:[291.7, 0.27155, 0.31914]},
  {ga:0.25, m:[0.62033, -1.58098, 13.92953, 5.46556, -49.64334, 154.03628], s:[295.8, 0.26433, 0.29596]},
  {ga:0.1602, m:[0.67118, -1.57574, 13.88332, 5.91355, -48.36191, 150.23993], s:[299.85, 0.25716, 0.27295]},
  {ga:0.0781, m:[0.72201, -1.57049, 13.83713, 6.36138, -47.08096, 146.44496], s:[304.0, 0.25, 0.25]},
];

function quadEaseOut(t, b, c, d) {
  t /= d;
  return -c * t * (t - 2) + b;
}

function quadEaseInOut(t, b, c, d) {
  t /= d / 2;
  if (t < 1) return (c / 2) * t * t + b;
  t -= 1;
  return (-c / 2) * (t * (t - 2) - 1) + b;
}

function loadImage(src) {
  return new Promise(function (resolve, reject) {
    const img = new Image();
    img.onload = function () { resolve(img); };
    img.onerror = function () { reject(new Error('could not load ' + src)); };
    img.src = src;
  });
}

class GtaMenuGlare {
  constructor(canvas, options) {
    options = options || {};
    this.canvas = canvas;
    this.ctx = canvas.getContext('2d');

    this.glintTextureSrc = options.glintTexture || 'menuapi/assets/glint.png';
    this.autoGlint = options.autoGlint !== false;
    this.fadeIn = options.fadeIn !== false;
    this.bleed = options.bleed === undefined ? DESIGN_BLEED : options.bleed;

    this.globePath = new Path2D(GLOBE_D);
    this.glarePath = new Path2D(GLARE_D);

    this.targetAngle = 0;
    this.position = 0;
    this.easedPosition = 0;
    this.contentAlpha = this.fadeIn ? 0 : 1;
    this.easeInCur = this.fadeIn ? 0 : 30;
    this.easeInDuration = 30;

    this.glintFrame = GLINT_LOOP_FRAME;
    this.glintPattern = null;

    this.running = false;
    this.accumulator = 0;
    this.lastTime = 0;
    this.raf = 0;

    this._tick = this._tick.bind(this);
  }

  load() {
    const self = this;
    return loadImage(this.glintTextureSrc).then(function (img) {
      self.glintPattern = self.ctx.createPattern(img, 'repeat');
      return self;
    });
  }

  start() {
    if (this.running) return this;
    this.running = true;
    this.lastTime = performance.now();
    this.accumulator = 0;
    this.raf = requestAnimationFrame(this._tick);
    return this;
  }

  stop() {
    this.running = false;
    if (this.raf) cancelAnimationFrame(this.raf);
    return this;
  }

  open() {
    this.easeInCur = 0;
    this.contentAlpha = 0;
    return this;
  }

  setHeading(angle, triggerGlint) {
    if (triggerGlint) this.triggerGlint();
    this.targetAngle = angle % 360;
    return this;
  }

  triggerGlint() {
    this.glintFrame = 2;
    return this;
  }

  toDataURL() {
    return this.canvas.toDataURL('image/png');
  }

  _resize() {
    const dpr = window.devicePixelRatio || 1;
    const rect = this.canvas.getBoundingClientRect();
    const w = Math.max(1, Math.round(rect.width * dpr));
    const h = Math.max(1, Math.round(rect.height * dpr));
    if (this.canvas.width !== w || this.canvas.height !== h) {
      this.canvas.width = w;
      this.canvas.height = h;
    }
  }

  _step() {
    if (this.easeInCur < this.easeInDuration) {
      this.contentAlpha =
        quadEaseOut(this.easeInCur++, 0, 100, this.easeInDuration) / 100;
    } else {
      this.contentAlpha = 1;
    }

    let targetPosition = (this.targetAngle % 180) / 180;
    if (this.targetAngle > 180) targetPosition = 1 - targetPosition;

    this.position += (targetPosition - this.position) / 16;
    this.easedPosition = quadEaseInOut(this.position, 0, 1, 1);

    this.glintFrame += 1;
    if (this.glintFrame === GLINT_LOOP_FRAME && this.autoGlint) {
      const span = GLINT_TOTAL_FRAMES - GLINT_LOOP_FRAME - GLINT_MIN_DELAY;
      this.glintFrame = Math.round(Math.random() * span) + GLINT_LOOP_FRAME;
    }
    if (this.glintFrame > GLINT_TOTAL_FRAMES) this.glintFrame = 1;
  }

  _tick(now) {
    if (!this.running) return;
    this.raf = requestAnimationFrame(this._tick);

    this.accumulator += now - this.lastTime;
    this.lastTime = now;
    if (this.accumulator > FRAME_MS * 5) this.accumulator = FRAME_MS * 5;

    let stepped = false;
    while (this.accumulator >= FRAME_MS) {
      this.accumulator -= FRAME_MS;
      this._step();
      stepped = true;
    }
    if (stepped) this.draw();
  }

  draw() {
    this._resize();
    const ctx = this.ctx;
    const w = this.canvas.width;
    const h = this.canvas.height;

    ctx.setTransform(1, 0, 0, 1, 0, 0);
    ctx.clearRect(0, 0, w, h);
    if (this.contentAlpha <= 0) return;

    ctx.save();
    ctx.scale(w / DESIGN_W, h / (DESIGN_H + this.bleed));
    ctx.imageSmoothingEnabled = true;
    ctx.imageSmoothingQuality = 'high';

    this._drawGlobe(ctx);
    this._drawGlint(ctx);
    this._drawGlare(ctx);

    ctx.restore();
  }

  _drawGlobe(ctx) {
    ctx.save();
    ctx.globalAlpha = GLOBE_ALPHA * this.contentAlpha;
    ctx.fillStyle = '#000';
    ctx.translate(GLOBE_X, GLOBE_Y);
    ctx.fill(this.globePath, 'evenodd');
    ctx.restore();
  }

  _drawGlint(ctx) {
    const frame = GLINT_FRAMES[this.glintFrame - 1];
    if (!frame || !this.glintPattern) return;

    ctx.save();
    ctx.beginPath();
    ctx.rect(0, 0, DESIGN_W, GLINT_CLIP_H);
    ctx.clip();

    if (frame.m && frame.ga > 0) {
      ctx.save();
      ctx.globalAlpha = frame.ga * this.contentAlpha;
      ctx.translate(GLOBE_X, GLOBE_Y);
      this.glintPattern.setTransform(new DOMMatrix(frame.m));
      ctx.fillStyle = this.glintPattern;
      ctx.fill(this.globePath, 'evenodd');
      ctx.restore();
    }

    if (frame.s) {
      const x = frame.s[0], sx = frame.s[1], sy = frame.s[2];
      ctx.save();
      ctx.globalCompositeOperation = 'lighter';
      ctx.globalAlpha = this.contentAlpha;
      ctx.translate(x, 65);
      ctx.scale(sx, sy);
      this.glintPattern.setTransform(new DOMMatrix([1, 0, 0, 1, -64, -8]));
      ctx.fillStyle = this.glintPattern;
      ctx.fillRect(-64, -8, 128, 16);
      ctx.restore();
    }

    ctx.restore();
  }

  _drawGlare(ctx) {
    const p = this.easedPosition;
    const x = GX1 + (GX2 - GX1) * p;
    const rotation = GR1 + (GR2 - GR1) * p;
    const alpha = (GA1 + (GA2 - GA1) * p) / 100;

    ctx.save();
    ctx.beginPath();
    ctx.rect(0, 0, DESIGN_W, DESIGN_H);
    ctx.clip();

    ctx.globalAlpha = alpha * this.contentAlpha;
    ctx.translate(x, GLARE_Y);
    ctx.rotate((rotation * Math.PI) / 180);

    const grad = ctx.createLinearGradient(
      GLARE_GRAD_FROM[0], GLARE_GRAD_FROM[1],
      GLARE_GRAD_TO[0], GLARE_GRAD_TO[1]
    );
    for (let i = 0; i < GLARE_RAMP.length; i++) {
      grad.addColorStop(
        i / (GLARE_RAMP.length - 1),
        'rgba(255,255,255,' + GLARE_RAMP[i] + ')'
      );
    }
    ctx.fillStyle = grad;
    ctx.fill(this.glarePath);
    ctx.restore();
  }
}
