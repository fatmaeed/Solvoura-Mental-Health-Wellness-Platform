import { jwtDecode } from "jwt-decode";

export interface IJWTClaims {
    id:string ,
    userId:string ,
    role:string ,
    email:string
}

