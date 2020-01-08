enum ResultType {
    Success,
    Error
}

export type ApiResult<T> =
    | { type: ResultType.Success; value: T }
    | { type: ResultType.Error; error: Error };
