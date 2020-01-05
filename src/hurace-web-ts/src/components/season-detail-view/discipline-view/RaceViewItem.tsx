import React from 'react';
import { Race } from '../../../interfaces/Race';
import styled from 'styled-components';
import { Card, DefaultLink } from '../../../theme/StyledComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faUser,
    faFlagCheckered
} from '@fortawesome/free-solid-svg-icons';

const RaceCard = styled(Card)`
    margin: 0px 10px 10px 0px;
    display: flex;
    flex-direction: column;
`;

export const RaceViewItem: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <DefaultLink to={`/season/${race.seasonId}/race/${race.id}`}>
            <RaceCard>
                <span>
                    <FontAwesomeIcon icon={faLocationArrow} />{' '}
                    {race.location.locationName}
                </span>
                <span>
                    <FontAwesomeIcon icon={faClock} />{' '}
                    {race.raceDate.toDateString()}
                </span>
                <span>
                    <FontAwesomeIcon icon={faUser} />{' '}
                    {race.gender.genderDescription}
                </span>
                <span>
                    <FontAwesomeIcon icon={faFlagCheckered} />{' '}
                    {race.raceState.raceStateDescription}
                </span>
            </RaceCard>
        </DefaultLink>
    );
};
