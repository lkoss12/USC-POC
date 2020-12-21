export interface AppUser extends User {
    ImpersonatedUser?: User
}

export interface User {
    Token: string
    UscId?: string,
    Fullname?: string,
}
