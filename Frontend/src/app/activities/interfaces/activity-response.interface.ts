import { SelectItem } from "../../shared/interfaces";

export interface ActivityReponse {
  id: number;
  name: string;
  client: string;
  description?: string;
  international: boolean;
  begin?: Date;
  end?: Date;
  price: number;
  musicians: SelectItem[]
}


