export interface EmptyResult {
    isSuccessful: boolean,
    clientErrorMessage: string
}

export interface Result<TBody> {
    isSuccessful: boolean,
    clientErrorMessage: string,
    value : TBody
}