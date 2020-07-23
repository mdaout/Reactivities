import React, { useState, FormEvent, useContext } from 'react'
import { Segment, Form, Button } from 'semantic-ui-react'
import { IActivity } from '../../../app/models/Activity';
import {v4 as uuid} from 'uuid';
import ActivityStore from '../../../app/stores/activitystore';
//import observer from '../dashboard/ActivityList';
import {observer} from 'mobx-react-lite';

interface IProps {
  //   setEditMode: (editMode: boolean) => void;
    activity: IActivity;
   // editActivity: (activity: IActivity) => void;  // now using editActivity from store
   // submitting: boolean; goes out with the line above
}

export const ActivityForm: React.FC<IProps> = ({
    activity: initialFormState,
   // editActivity,    // now using editActivity from store see 4 lines below
   // submitting  // goes out as as editActivity freom store comes in. see 3 lines below
}) => {
    const activityStore = useContext(ActivityStore);
    const {creatActivity, editActivity,submitting, cancelFormOpen } = activityStore;;
    const initializeForm = () => {
        if (initialFormState) {
            return initialFormState
        } else {
            return {
                id: '',
                title: '',
                description: '',
                category: '',
                date: '',
                city: '',
                venue: ''
            }
        }

    };
    const [activity, setActivity] = useState<IActivity>(initializeForm)

    const handelSubmit = () => {
        if(activity.id.length === 0) {

            let newActivity = {...activity, id: uuid()
            }
            creatActivity(newActivity);
        } else {
            editActivity(activity);
        }
    }
    const handleInputChange = (event: FormEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        // console.log(event.currentTarget);
        const { name, value } = event.currentTarget;
        setActivity({ ...activity, [name]: value })
    }
    return (
        <Segment clearing>
            <Form onSubmit={handelSubmit}>
                <Form.Input
                    name='title'
                    onChange={handleInputChange}
                    placeholder='Title'
                    value={activity.title} />

                <Form.TextArea
                    name='description'
                    onChange={handleInputChange} rows={2}
                    placeholder='Description'
                    value={activity.description} />

                <Form.Input
                    name='category'
                    onChange={handleInputChange}
                    placeholder='Category'
                    value={activity.category} />

                <Form.Input
                    name='date'
                    onChange={handleInputChange}
                    type='datetime-local'
                    placeholder='Date'
                    value={activity.date} />

                <Form.Input
                    name='city'
                    onChange={handleInputChange}
                    placeholder='City'
                    value={activity.city} />

                <Form.Input
                    name='venue'
                    onChange={handleInputChange}
                    placeholder='Venue'
                    value={activity.venue} />

                <Button 
                loading={submitting} 
                floated='right'
                 type='submit' 
                 positive content='Submit' />

                <Button 
                // onClick={() => setEditMode(false)}
                 onClick={cancelFormOpen}
                 floated='right'
                  type='button' 
                  positive content='Cancel' />
            </Form>
        </Segment>
    )
}

export default observer(ActivityForm); 