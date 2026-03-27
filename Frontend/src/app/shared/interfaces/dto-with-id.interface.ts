export interface DtoWithId {
  id: number;
  [key : string] : any;
}

export interface SelectItem {
  id: number;
  text : string;
}

export interface CheckedItem {
  id: number;
  text : string;
  checked : boolean;
}
