import React from 'react'
import { Item, Label, Button, Segment, Icon } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { IActivity } from '../../../app/models/Activity';
import {format} from 'date-fns';


export const ActivityListItem: React.FC<{ activity: IActivity }> = ({
    activity }) => {
    return (
        <Segment.Group>
            <Segment>
            <Item.Group>
                <Item>
                    <Item.Image size='tiny' circular src='/assets/user.png' />
                    <Item.Content>
                        <Item.Header as='a'>{activity.title}</Item.Header>
                        {/* <Item.Meta>{activity.date}</Item.Meta> */}
                        <Item.Description>
                            Hosted By Dukes Mayo
                        </Item.Description>
                        <Item.Extra>
                            <Label basic content={activity.category} />
                        </Item.Extra>
                    </Item.Content>
                </Item>
                </Item.Group>
            </Segment>
            <Segment>
            <Icon name='clock' /> {format(activity.date, 'eeee do MMMM')} at {format(activity.date, 'h:mm a')}
                <Icon name='marker' /> {activity.venue}, {activity.city}
            </Segment>
            <Segment secondary>
                Attendees will go here
            </Segment>
            <Segment clearing>
                <span>{activity.description}</span>
                <Button
                    as={Link}
                    to={`/activities/${activity.id}`}
                    floated='right'
                    content='View'
                    color='blue'
                />
            </Segment>
        </Segment.Group>


    )
}
