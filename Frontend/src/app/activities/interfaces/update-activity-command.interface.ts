export interface UpdateActivityCommand {
  id: number;
  name: string;
  client: string;
  description?: string;
  international: boolean;
  begin?: Date;
  end?: Date;
  musiciansId: number[];
}
