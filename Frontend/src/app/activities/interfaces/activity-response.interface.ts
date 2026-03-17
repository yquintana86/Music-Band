export interface ActivityReponse {
  id: number;
  name: string;
  client: string;
  description?: string;
  international: boolean;
  begin?: Date;
  end?: Date;
}
