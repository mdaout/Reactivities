import React, { SyntheticEvent } from 'react'
import { Grid } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/Activity';
import ActivityList from './ActivityList';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../forms/ActivityForm';

interface IProps {

    activities: IActivity[];
    selectActivity: (id: string) => void;
    selectedActivity: IActivity | null;
    editMode: boolean;
    setEditMode: (editMode: boolean) => void;
    setselectedActivity: (activity: IActivity | null) => void;
    createActivity: (activity: IActivity) => void;
    editActivity: (activity: IActivity) => void;
    deleteActivity: (e: SyntheticEvent<HTMLButtonElement>, id: string) => void;
    submitting: boolean;
    target: string;
}

export const ActivityDashboard: React.FC<IProps> = ({
    activities,
    selectActivity,
    selectedActivity,
    editMode,
    setEditMode,
    setselectedActivity,
    createActivity,
    editActivity,
    deleteActivity,
    submitting,
    target

}) => {
    return (
        <Grid>
            <Grid.Column width={10}>=
                <ActivityList 
                activities={activities} 
                selectActvitiy={selectActivity} 
                deleteActivity={deleteActivity}
                submitting={submitting}
                target={target} />

                {/* <List>
                    {activities.map((activity) => (
                    // <li key={value.id}>{value.id + "-- " + value.name}</li>
                    <List.Item key={activity.id}>{activity.title}</List.Item>
                    ))}
                </List> */}
            </Grid.Column>
            <Grid.Column width={6}>
                {selectedActivity && !editMode && (
                    <ActivityDetails activity={selectedActivity} setEditMode={setEditMode}
                        setselectedActivity={setselectedActivity}
                    />)}

                {/* // below when we give the form a key and the key chnges an intitial occurs 
         // selectedActivity  this test for existense or not null */}
                {editMode && <ActivityForm
                    key={selectedActivity && selectedActivity.id || 0}
                    setEditMode={setEditMode}
                    activity={selectedActivity!}
                    createActivity={createActivity}
                    editActivity={editActivity} 
                    submitting={submitting}
                    />}
            </Grid.Column>
        </Grid>
    )
}

export default ActivityDashboard