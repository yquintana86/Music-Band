export interface User {
  sub:        number;
  email:      string;
  given_name: string;
  permission: string[];
  aud:        string;
  iss:        string;
}
