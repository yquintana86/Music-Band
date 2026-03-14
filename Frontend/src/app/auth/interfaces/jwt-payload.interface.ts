import { User } from "./user.interface";

export interface JwtPayload {
  sub:        number;
  email:      string;
  given_name: string;
  permission: string[];
  aud:        string;
  iss:        string;
  exp:        number;
  iat:        number;
  nbf:        number;
}
