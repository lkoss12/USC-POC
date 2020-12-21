export interface Token {
    typ?: string;
    sub?: string;
    nameid?: string;
    nbf: string;
    exp: string;
    iat: string;
}
