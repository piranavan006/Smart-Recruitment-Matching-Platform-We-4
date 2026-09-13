export interface ContactRequestResponse {
  contactRequestId: number;
  senderId: number;
  receiverId: number;
  message: string;
  status: string;
  createdAt: string;
}

export interface CreateContactRequest {
  receiverId: number;
  message: string;
}

export interface UpdateContactRequestStatus {
  status: 'Accepted' | 'Declined';
}
