﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WFCAD {
    /// <summary>
    /// 図形群クラス
    /// </summary>
    public class Shapes : IShapes {
        private List<IShape> FShapes = new List<IShape>();
        private bool FVisible = true;

        #region プロパティ

        /// <summary>
        /// 表示状態
        /// </summary>
        public bool Visible {
            get => FVisible;
            set {
                FVisible = value;
                foreach (IShape wShape in FShapes.Where(x => x.IsSelected)) {
                    wShape.Visible = FVisible;
                }
            }
        }

        #endregion プロパティ

        #region メソッド

        /// <summary>
        /// 描画します
        /// </summary>
        public Bitmap Draw(Bitmap vBitmap) {
            FShapes.ForEach(x => x.Draw(vBitmap));
            return vBitmap;
        }

        /// <summary>
        /// 選択します
        /// </summary>
        public void Select(Point vCoordinate, bool vIsMultiple) {
            if (FShapes.Where(x => x.IsSelected).ToList().Count >= 2) {
                if (FShapes.Any(x => x.IsHit(vCoordinate))) {
                    foreach (IShape wShape in FShapes) {
                        bool wIsHit = wShape.IsHit(vCoordinate);
                        wShape.IsSelected = wShape.IsSelected || wIsHit;
                    }
                } else {
                    FShapes.ForEach(x => x.IsSelected = false);
                }
            } else {
                bool wHasSelected = false;
                foreach (IShape wShape in Enumerable.Reverse(FShapes)) {
                    if (wHasSelected) {
                        wShape.IsSelected = false;
                    } else {
                        bool wIsHit = wShape.IsHit(vCoordinate);
                        if (vIsMultiple) {
                            wShape.IsSelected = wShape.IsSelected || wIsHit;
                        } else {
                            wShape.IsSelected = wIsHit;
                            if (wShape.IsSelected) {
                                wHasSelected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 追加します
        /// </summary>
        public void Add(IShape vShape) => FShapes.Add(vShape);

        /// <summary>
        /// 移動します
        /// </summary>
        public void Move(Size vSize) {
            foreach (IShape wShape in FShapes.Where(x => x.IsSelected)) {
                wShape.Move(vSize);
            }
        }

        /// <summary>
        /// 最前面に移動します
        /// </summary>
        public void MoveToFront() => FShapes = FShapes.OrderBy(x => x.IsSelected).ToList();

        /// <summary>
        /// 最背面に移動します
        /// </summary>
        public void MoveToBack() => FShapes = FShapes.OrderByDescending(x => x.IsSelected).ToList();

        /// <summary>
        /// 複製します
        /// </summary>
        public void Clone() {
            var wClonedShapes = new List<IShape>();
            foreach (IShape wShape in FShapes.Where(x => x.IsSelected)) {
                IShape wClone = wShape.DeepClone();

                // 選択状態をスイッチします
                wShape.IsSelected = false;
                wClone.IsSelected = true;

                // 右下方向
                var wMovingSize = new Size(10, 10);
                wClone.StartPoint += wMovingSize;
                wClone.EndPoint += wMovingSize;
                wClonedShapes.Add(wClone);
            }
            FShapes.AddRange(wClonedShapes);
        }

        /// <summary>
        /// 削除します
        /// </summary>
        public void Remove() => FShapes.RemoveAll(x => x.IsSelected);

        /// <summary>
        /// クリアします
        /// </summary>
        public void Clear() => FShapes.Clear();

        /// <summary>
        /// 自身のインスタンスを複製します
        /// </summary>
        public IShapes DeepClone() {
            var wClone = new Shapes();
            foreach (IShape wShape in FShapes) {
                wClone.Add(wShape.DeepClone());
            }
            return wClone;
        }

        #endregion メソッド

    }
}