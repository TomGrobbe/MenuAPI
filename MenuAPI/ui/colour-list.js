// Port of Rockstar's COLOUR_SWITCHER scaleform to HTML. Method names, numbers and units are the
// scaleform's own, so what the C# side sends maps across one for one.


const COMPONENT_W = 288;
const TITLE_H = 50;
const PALETTE_H = 74;

const SWATCH_W = 30;
const HIGHLIGHT_H = 5;
const SWATCH_COLOUR_H = 30;

const VISIBLE_ITEMS = 9;
const STRIP_W = VISIBLE_ITEMS * SWATCH_W;

const BAR_X = 7;
const BAR_Y = 30;
const BAR_W = 274;
const BAR_H = 6;

const ARROW_LEFT_X = 7.35;
const ARROW_RIGHT_X = 282;
const ARROW_Y = 14.6;

const HIGHLIGHT_DROP = 10;
const HIGHLIGHT_DURATION = 0.3;

const BAR_TWEEN_DURATION = 0.175;

function circEaseOut(t, b, c, d) {
  t = t / d - 1;
  return c * Math.sqrt(1 - t * t) + b;
}

function quadEaseOut(t, b, c, d) {
  t /= d;
  return -c * t * (t - 2) + b;
}

function el(tag, cls, parent) {
  const node = document.createElement(tag);
  if (cls) node.className = cls;
  if (parent) parent.appendChild(node);
  return node;
}

function clamp(v, lo, hi) {
  return Math.max(lo, Math.min(v, hi));
}

class GtaColourList {
  constructor(root, options) {
    options = options || {};
    this.root = root;
    this.arrowsAllowed = options.arrows !== false;
    this.visibleItems = options.visibleItems || VISIBLE_ITEMS;
    this.onSelect = options.onSelect || function () {};
    this.onScroll = options.onScroll || function () {};

    this.colourData = [];
    this.swatches = [];
    this.highlightIndex = 0;
    this.highlightPosIndex = 0;
    this.topEdge = 0;
    this.pcActive = false;

    this.tweens = [];
    this.raf = 0;
    this._tick = this._tick.bind(this);

    this._build();
  }

  _build() {
    const r = this.root;
    r.classList.add('gcl');
    r.innerHTML = '';

    this.titleEl = el('div', 'gcl__title', r);
    this.titleEl.hidden = true;
    this.titleLabel = el('div', 'gcl__label', this.titleEl);
    this.minLabel = el('div', 'gcl__min', this.titleEl);
    this.maxLabel = el('div', 'gcl__max', this.titleEl);

    this.barEl = el('div', 'gcl__bar', this.titleEl);
    this.barBlack = el('div', 'gcl__bar-black', this.barEl);
    this.barAlpha = el('div', 'gcl__bar-alpha', this.barEl);
    this.barFill = el('div', 'gcl__bar-fill', this.barEl);
    this.barWidth = BAR_W;

    this.paletteEl = el('div', 'gcl__palette', r);
    this.paletteEl.hidden = true;
    this.stripEl = el('div', 'gcl__strip', this.paletteEl);
    this.nameLabel = el('div', 'gcl__name', this.paletteEl);

    this.leftArrow = el('button', 'gcl__arrow gcl__arrow--left', this.paletteEl);
    this.rightArrow = el('button', 'gcl__arrow gcl__arrow--right', this.paletteEl);
    this.leftArrow.type = this.rightArrow.type = 'button';
    this.leftArrow.hidden = this.rightArrow.hidden = true;
    this.leftArrow.addEventListener('click', () => this.onScroll(-1));
    this.rightArrow.addEventListener('click', () => this.onScroll(1));

    this.stripEl.addEventListener('mouseleave', () => {
      if (this.pcActive) this.onSelect(-1);
    });
  }

  SET_IS_PC(isPc) {
    this.pcActive = !!isPc && this.arrowsAllowed;
    this.leftArrow.hidden = this.rightArrow.hidden = !this.pcActive;
    this.root.classList.toggle('gcl--pc', this.pcActive);
    return this;
  }

  SET_TITLE(title, paletteLabel, percent, showArrows) {
    if (paletteLabel !== undefined) this.nameLabel.textContent = paletteLabel;

    if (percent === undefined || percent === -1 || isNaN(percent)) {
      this.titleEl.hidden = true;
      this.root.classList.add('gcl--no-title');
    } else {
      if (title !== undefined) this.titleLabel.textContent = title;
      this.minLabel.textContent = '0%';
      this.maxLabel.textContent = '100%';
      this.percent(percent);
      this.titleEl.hidden = false;
      this.root.classList.remove('gcl--no-title');
    }

    if (showArrows) this.SET_IS_PC(true);
    return this;
  }

  SHOW_OPACITY(show, opacityPosTop) {
    this.titleEl.hidden = !show;
    this.root.classList.toggle('gcl--no-title', !show);
    this.root.style.setProperty(
      '--palette-offset', opacityPosTop ? 0 : PALETTE_H
    );
    return this;
  }

  percent(p, tween) {
    const clamped = clamp(p, 0, 100);
    const target = Math.round(this.barWidth * (clamped / 100));
    if (tween) {
      this._tween(this.barFill, this.barFillWidth || 0, target,
        BAR_TWEEN_DURATION, quadEaseOut, (node, v) => {
          node.style.width = 'calc(' + v + 'px * var(--u))';
        });
    } else {
      this.barFill.style.width = 'calc(' + target + 'px * var(--u))';
    }
    this.barFillWidth = target;
    return this;
  }

  SET_DATA_SLOT(index, r, g, b) {
    this.colourData[index] = [index, r, g, b];
    return this;
  }

  SET_DATA_SLOT_EMPTY() {
    this.tweens.length = 0;
    this.stripEl.innerHTML = '';
    this.colourData = [];
    this.swatches = [];
    this.paletteEl.hidden = true;
    return this;
  }

  DISPLAY_VIEW() {
    const count = Math.min(this.colourData.length, this.visibleItems);
    this.stripEl.innerHTML = '';
    this.swatches = [];

    for (let i = 0; i < count; i++) {
      const swatch = el('div', 'gcl__swatch', this.stripEl);
      const highlight = el('div', 'gcl__highlight', swatch);
      const colour = el('div', 'gcl__colour', swatch);

      swatch.dataset.index = String(i);
      highlight.hidden = true;

      colour.addEventListener('mouseenter', () => {
        if (this.pcActive) this.onSelect(this._dataIndexAt(i));
      });
      colour.addEventListener('click', () => {
        this.onSelect(this._dataIndexAt(i));
      });

      this.swatches.push({ root: swatch, highlight: highlight, colour: colour });
      this.itemSetData(i, this.swatches[i], this.colourData[i]);
    }

    this.repositionPalettes();
    this.paletteEl.hidden = false;
    this._startLoop();
    return this;
  }

  UPDATE_SLOT(index, r, g, b) {
    this.SET_DATA_SLOT(index, r, g, b);
    const local = index - this.topEdge;
    if (this.swatches[local]) {
      this.itemSetData(local, this.swatches[local], this.colourData[index]);
    }
    return this;
  }

  CLEAR_HIGHLIGHT() {
    this.highlightIndex = 0;
    this.highlightPosIndex = 0;
    this.topEdge = 0;
    return this;
  }

  SET_HIGHLIGHT(index) {
    const total = this.colourData.length;
    if (!total) return this;

    index = clamp(index, 0, total - 1);
    let local = index;
    let firstVisible = this.topEdge;

    if (total > this.visibleItems) {
      if (local > this.topEdge + this.visibleItems - 1) {
        firstVisible = local - (this.visibleItems - 1);
        this.topEdge = firstVisible;
        local = this.visibleItems - 1;
      } else if (local < this.topEdge) {
        firstVisible = local;
        this.topEdge = firstVisible;
        local = 0;
      } else {
        firstVisible = this.topEdge;
        local -= this.topEdge;
      }
      for (let i = 0; i < this.swatches.length; i++) {
        this.itemSetData(i, this.swatches[i], this.colourData[firstVisible + i]);
      }
    }

    for (let i = 0; i < this.swatches.length; i++) {
      const on = i === local;
      const highlight = this.swatches[i].highlight;
      highlight.hidden = !on;
      if (on) {
        if (this.highlightPosIndex !== local) {
          this._tween(highlight, HIGHLIGHT_DROP, 0,
            HIGHLIGHT_DURATION, circEaseOut, (node, v) => {
              node.style.transform = 'translateY(calc(' + v + 'px * var(--u)))';
            });
        } else {
          this.tweens = this.tweens.filter((t) => t.node !== highlight);
          highlight.style.transform = 'translateY(0)';
        }
      }
    }

    this.highlightIndex = index;
    this.highlightPosIndex = local;
    return this;
  }

  itemSetData(i, swatch, data) {
    if (!swatch) return;
    if (!data) {
      swatch.root.hidden = true;
      return;
    }
    swatch.root.hidden = false;
    const r = data[1], g = data[2], b = data[3];
    if (r !== undefined) {
      swatch.colour.style.background = 'rgb(' + r + ',' + g + ',' + b + ')';
    }
  }

  repositionPalettes() {
    const count = this.swatches.length;
    if (!count) return this;
    const w = count <= this.visibleItems
      ? (this.visibleItems * SWATCH_W) / count
      : SWATCH_W;

    for (let i = 0; i < count; i++) {
      const s = this.swatches[i];
      s.root.style.setProperty('--sw', w);
      s.root.style.left = 'calc(' + (i * w) + 'px * var(--u))';
    }
    return this;
  }

  _dataIndexAt(local) {
    return this.topEdge + local;
  }

  _tween(node, from, to, duration, ease, apply) {
    this.tweens = this.tweens.filter((t) => t.node !== node);
    this.tweens.push({
      node: node, from: from, to: to,
      duration: duration * 1000, start: performance.now(),
      ease: ease, apply: apply,
    });
    apply(node, from);
    this._startLoop();
  }

  _startLoop() {
    if (!this.raf) this.raf = requestAnimationFrame(this._tick);
  }

  _tick(now) {
    this.raf = 0;
    for (let i = this.tweens.length - 1; i >= 0; i--) {
      const t = this.tweens[i];
      const elapsed = now - t.start;
      if (elapsed >= t.duration) {
        t.apply(t.node, t.to);
        this.tweens.splice(i, 1);
      } else {
        t.apply(t.node, t.ease(elapsed / 1000, t.from, t.to - t.from, t.duration / 1000));
      }
    }
    if (this.tweens.length) this._startLoop();
  }

  destroy() {
    if (this.raf) cancelAnimationFrame(this.raf);
    this.raf = 0;
    this.tweens.length = 0;
    return this;
  }
}

Object.assign(GtaColourList.prototype, {
  setIsPc: GtaColourList.prototype.SET_IS_PC,
  setTitle: GtaColourList.prototype.SET_TITLE,
  showOpacity: GtaColourList.prototype.SHOW_OPACITY,
  setDataSlot: GtaColourList.prototype.SET_DATA_SLOT,
  clearData: GtaColourList.prototype.SET_DATA_SLOT_EMPTY,
  displayView: GtaColourList.prototype.DISPLAY_VIEW,
  updateSlot: GtaColourList.prototype.UPDATE_SLOT,
  clearHighlight: GtaColourList.prototype.CLEAR_HIGHLIGHT,
  setHighlight: GtaColourList.prototype.SET_HIGHLIGHT,
});
