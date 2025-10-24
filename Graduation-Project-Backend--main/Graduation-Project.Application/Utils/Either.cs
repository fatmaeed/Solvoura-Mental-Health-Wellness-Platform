using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.Application.Utils {

    public delegate IActionResult FoldAction<Return>(Return left);
    public delegate Task<IActionResult> FoldActionAsync<Return>(Return left);

    public delegate void Fold<Return>(Return left);

    public class Either<Left, Right> {
        private readonly bool isSuccess = false;
        private readonly Left left;
        private readonly Right right;

        private Either(Left left) {
            this.left = left;
            isSuccess = false;
        }

        private Either(Right right) {
            this.right = right;
            isSuccess = true;
        }

        public IActionResult Fold(FoldAction<Left> left, FoldAction<Right> right) {
            if (isSuccess) {
                return right(this.right);
            } else {
                return left(this.left);
            }
        }
        public Task<IActionResult> Fold(FoldActionAsync<Left> left, FoldActionAsync<Right> right) {
            if (isSuccess) {
                return right(this.right);
            } else {
                return left(this.left);
            }
        }
        public void Fold(Fold<Left> left, Fold<Right> right) {
            if (isSuccess) {
                right(this.right);
            } else {
                left(this.left);
            }
        }

        public static Either<Left, Right> SendLeft(Left left) {
            return new Either<Left, Right>(left);
        }

        public static Either<Left, Right> SendRight(Right right) {
            return new Either<Left, Right>(right);
        }
    }
}