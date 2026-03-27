export interface CreateActivityCommand {
  name: string;
  client: string;
  description?: string;
  international: boolean;
  begin?: Date;
  end?: Date;
  musiciansId: number[];
  price: number;
}
