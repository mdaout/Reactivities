import React, { useState, useEffect, Fragment } from 'react';
// import logo from './logo.svg';
import axios from 'axios';
import { Container } from 'semantic-ui-react'
import { IActivity } from '../models/Activity';
import NavBar from '../../features/nav/NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';

const App = () => {

  const [activities, setActivities] = useState<IActivity[]>([])
  const [selectedActivity, setselectedActivity] = useState<IActivity | null>(null)
  const [editMode, setEditMode] = useState(false);

  const handelSelectedActivity = (id: string) => {
    setselectedActivity(activities.filter(a => a.id === id)[0])
    setEditMode(false);  // when view button clicked turn edit off
  }

  const handleOpenCreateForm = () => {
    setselectedActivity(null);
    setEditMode(true);
  }

  // ... is a spreader it creates a new array. below creates an array an adds ourcnew activity to it
  const handelCreateActivity = (activity: IActivity) => {
    setActivities([...activities, activity])
    setselectedActivity(activity); // set detial to disply our newly created activity
    setEditMode(false);  // trun off edit mode
  }

  // below creates an array of all the activites not equaling activit in quetio then addsa our activit to it
  const handelEditActivity = (activity: IActivity) => {
    setActivities([...activities.filter(a => a.id !== activity.id), activity])
    setselectedActivity(activity); // set detial to disply our newly created activity
    setEditMode(false);  // trun off edit mode
  }
const handleDeleteActivity= (id: string) =>
{
  setActivities([...activities.filter(a => a.id !== id)])

}

  useEffect(() => {
    axios
      .get<IActivity[]>('http://localhost:5000/api/activities')
      .then((response) => {
         //  console.log(response);
        let activites: IActivity[] = [];
        response.data.forEach(activity => {
          activity.date = activity.date.split('.')[0];
          activities.push(activity);
        })
        setActivities(response.data)
       // setActivities(activities)
      });
  }, []);

  return (
    <Fragment>
      <NavBar openCreateForm={handleOpenCreateForm} />
      <Container style={{ marginTop: '7em' }} >
        <ActivityDashboard activities={activities}
          selectActivity={handelSelectedActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          setEditMode={setEditMode}
          setselectedActivity={setselectedActivity} 
          createActivity={handelCreateActivity}
          editActivity={handelEditActivity}
          deleteActivity={handleDeleteActivity}
          />
      </Container>

    </Fragment>
  );

}

export default App;
