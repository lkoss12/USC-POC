export interface Token {
    typ?: string;
    sub?: string;
    nameid?: string;
    claims?: string;
    nbf: string;
    exp: string;
    iat: string;
}
