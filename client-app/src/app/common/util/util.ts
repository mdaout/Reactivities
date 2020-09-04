import { IActivity, IAttendee } from "../../models/activity";
import { IUser } from "../../models/User";

export const combineDateAndTime = (date : Date , time: Date) => {

    const timestring = time.getHours() + ':' +  time.getMinutes() + ':00';
    const  year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day  = date.getDate();
    const dateString = `${year}-${month}-${day}`;
    return new Date(dateString + ' ' + timestring);
}

export const setActivityProps = (activity: IActivity, user: IUser) => {

 //   activity.date = new Date(activity.date);
    // If User in list of users going set to true
    activity.isGoing = activity.attendees.some(a => a.userName === user.userName )
    // If User in list of users going set to true
    activity.isHost = activity.attendees.some( a => a.userName === user.userName && a.ishost )
   console.log( 'activity set' + activity);
    return activity;
}

export const createAttendee = (user: IUser): IAttendee => {
    return {
        displayName: user.displayName,
        ishost: false,
        userName: user.userName,
        image: user.image!
    }
}