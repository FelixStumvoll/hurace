import React, { useState, useEffect } from 'react';
import { Race } from '../../interfaces/Race';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faSkiing,
    faUser,
    faFlagCheckered
} from '@fortawesome/free-solid-svg-icons';
import { setStateAsync } from '../../common/stateSetter';
import { getRaceDetails } from '../../api';
import { HeaderCard } from '../HeaderCard';

const DetailPanel = styled.div`
    display: flex;
`;

const InfoContent = styled.div`
    display: grid;
    grid-template-rows: repeat(5, auto);
    column-gap: 5px;
    grid-template-columns: 30px auto;
`;

const DescriptionCardWrapper = styled.div`
    margin-left: 10px;
`;

const DetailText = styled.div`
    height: fit-content;
    margin: auto 0 auto 0;
`;

const HeaderText = styled(DetailText)`
    font-size: 25px;
    font-weight: bold;
`;

const RaceDescription = styled.div`
    grid-area: description;
    word-wrap: break-word;
    overflow: auto;
`;

const DetailIcon = styled(FontAwesomeIcon)<{ fontSize: number }>`
    font-size: ${props => props.fontSize}px;
    margin: auto;
`;

export const RaceDetailPanel: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [race, setRace] = useState<Race | undefined>(undefined);

    useEffect(() => {
        if (race !== undefined) return;
        setStateAsync(setRace, getRaceDetails(raceId));
    }, [race, raceId]);

    return race ? (
        <DetailPanel>
            <HeaderCard headerText="Stammdaten">
                <InfoContent>
                    <DetailIcon fontSize={25} icon={faLocationArrow} />
                    <HeaderText>{race?.location.locationName}</HeaderText>
                    <DetailIcon fontSize={16} icon={faSkiing} />
                    <DetailText>{race?.discipline.disciplineName}</DetailText>
                    <DetailIcon fontSize={16} icon={faFlagCheckered} />
                    <DetailText>
                        {race?.raceState.raceStateDescription}
                    </DetailText>
                    <DetailIcon fontSize={16} icon={faUser} />
                    <DetailText>{race?.gender.genderDescription}</DetailText>
                    <DetailIcon fontSize={16} icon={faClock} />
                    <DetailText>{race?.raceDate.toDateString()}</DetailText>
                </InfoContent>
            </HeaderCard>
            <DescriptionCardWrapper>
                <HeaderCard headerText="Beschreibung">
                    {race?.raceDescription && (
                        <RaceDescription>
                            {race?.raceDescription}
                        </RaceDescription>
                    )}
                </HeaderCard>
            </DescriptionCardWrapper>
        </DetailPanel>
    ) : (
        <div />
    );
};
